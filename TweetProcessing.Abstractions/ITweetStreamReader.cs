namespace TweetProcessing.Abstractions
{
    public interface ITweetStreamReader
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}
