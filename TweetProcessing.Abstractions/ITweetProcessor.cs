namespace TweetProcessing.Abstractions
{
    public interface ITweetProcessor
    {
        public Task StartAsync(CancellationToken cancellationToken);
    }
}