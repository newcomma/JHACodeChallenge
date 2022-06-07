namespace TweetProcessing.Abstractions
{
    public interface ITweetQueueConsumer
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}
