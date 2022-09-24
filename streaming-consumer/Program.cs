
using System;
using System.Buffers;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddTransient<IStreamService, StreamService>();
            }).UseConsoleLifetime();

        var host = builder.Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            var message = Console.ReadLine();
            try
            {
                if (message == "Y")
                {
                    var streamService = services.GetRequiredService<IStreamService>();

                    //testing endpoint :
                    //Note : Results will be partial when you invoke the url on browser. http://localhost:5296/api/stream/GetStudentStream3

                    await streamService.GetStream("http://localhost:5296/api/stream/GetStudentStream3").ConfigureAwait(false);
                    //await foreach (var c in streamService.GetStream("http://localhost:5296/api/stream/GetStudentStream3"))
                    //{
                    //    Console.WriteLine(c);
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        Console.ReadLine();
    }

}
