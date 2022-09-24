public interface IStreamService
{
    //IAsyncEnumerable<string> GetStream(string url);
    Task GetStream(string url);
}
