namespace TweetProcessing.Abstractions
{
    public interface ITweetListener
    {
        public Task StartAsync(CancellationToken cancellationToken);
    }
}