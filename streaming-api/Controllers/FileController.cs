using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;

namespace streaming_api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StreamController : ControllerBase
{
    private readonly ILogger<StreamController> _logger;
    private static List<Student> students = new List<Student>() {

        new Student(){ Name= "Name 1" },
        new Student(){ Name= "Name 2" },
        new Student(){ Name= "Name 3" },
        new Student(){ Name= "Name 4" },
        new Student(){ Name= "Name 5" },
        new Student(){ Name= "Name 6" },
        new Student(){ Name= "Name 7" },
        new Student(){ Name= "Name 8" },
        new Student(){ Name= "Name 9" }
    };

    public StreamController(ILogger<StreamController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("[action]")]
    public async IAsyncEnumerable<string> GetFileStream1()
    {
        using StreamReader reader = System.IO.File.OpenText(AppContext.BaseDirectory + "zero_to_one.dxf");
        while (!reader.EndOfStream)
        {
            await Task.Delay(100);
            yield return await reader.ReadLineAsync();
        }

        //foreach (var line in System.IO.File.ReadLines(AppContext.BaseDirectory + "zero_to_one.dxf"))
        //{
        //    await Task.Delay(25);
        //    Console.WriteLine(line);
        //    yield return line;
        //}
    }

    [HttpGet]
    [Route("[action]")]
    public async IAsyncEnumerable<int> GetNumberStream2()
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(100);
            yield return i;
        }
    }

    [HttpGet]
    [Route("[action]")]
    public async IAsyncEnumerable<Student> GetStudentStream3()
    {
        await foreach (var student in students.ToAsyncEnumerable<Student>())
        {
            yield return student;
            await Task.Delay(500);
        }
    }
}