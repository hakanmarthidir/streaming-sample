using Microsoft.AspNetCore.Mvc;

namespace streaming_api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async IAsyncEnumerable<string> GetFileStream()
    {
        string dxfLine;
        var streamReader = new StreamReader(AppContext.BaseDirectory + "zero_to_one.dxf");
        while ((dxfLine = await streamReader.ReadLineAsync()) != null)
        {
            yield return dxfLine;
        }
    }

    //Consume
    //var lines = GetFileStream()
    //await foreach(var line in lines)
    //{

    //}
}

