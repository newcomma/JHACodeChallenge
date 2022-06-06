namespace TweetProcessing.Abstractions
{
    public interface ITweetStream
    {
        IAsyncEnumerable<TweetDto> ReadAsync(CancellationToken cancellationToken);
    }
}
