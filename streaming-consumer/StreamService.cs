using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

public class Student
{
    public string Name { get; set; }
}

public class StreamService : IStreamService
{
    private readonly IHttpClientFactory _clientFactory;

    public StreamService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task GetStream(string url)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        //1- We got the data in one piece
        //var students = await response.Content.ReadFromJsonAsync<IAsyncEnumerable<Student>>().ConfigureAwait(false);
        //await foreach (Student s in students)
        //{
        //    Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] {s.Name}");
        //}

        //2- We got the data in one piece
        //using var stream = await response.Content.ReadAsStreamAsync();
        //using var streamReader = new StreamReader(stream, Encoding.UTF8);
        //while (!streamReader.EndOfStream)
        //{
        //    var line = await streamReader.ReadLineAsync();
        //    Console.WriteLine(line);
        //    yield return line;


        //3- Buffer Settings
        //DefaultBufferSize property: it must be greater than zero. If you give high buffer data will be reaching in batches.
        using Stream responseStream = await response.Content.ReadAsStreamAsync();
        await foreach (Student s in JsonSerializer.DeserializeAsyncEnumerable<Student>(
            responseStream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultBufferSize = 5

            }))
        {
            Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] {s.Name}");
        }

        //Check the times and buffersize. it is displaying the data arrived times.

        //BufferSize : 5 -> Result on Console - with Task.Delay
        //[10:24:02.841] Name 1
        //[10:24:02.850] Name 2
        //[10:24:03.832] Name 3
        //[10:24:04.833] Name 4
        //[10:24:04.833] Name 5
        //[10:24:05.334] Name 6
        //[10:24:06.337] Name 7
        //[10:24:06.337] Name 8
        //[10:24:06.853] Name 9

        //BufferSize : 100 -> Result on Console - with Task.Delay
        //[10:31:43.487] Name 1

        //[10:31:43.496] Name 2
        //[10:31:43.496] Name 3
        //[10:31:43.496] Name 4
        //[10:31:43.496] Name 5
        //[10:31:43.496] Name 6
        //[10:31:43.496] Name 7

        //[10:31:44.453] Name 8
        //[10:31:44.453] Name 9
    }
}