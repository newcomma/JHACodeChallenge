namespace TweetProcessing.Abstractions
{
    /// <summary>
    /// Represents an asyncronous publisher of <see cref="TweetDto"/> that
    /// have been received from the Twitter Sample Stream.
    /// </summary>
    public interface ITweetStream
    {
        /// <summary>
        /// Returns <see cref="TweetDto"/>s asynchronously as they are received from
        /// the Twitter Sample Stream.
        /// </summary>
        IAsyncEnumerable<TweetDto> ReadAsync(CancellationToken cancellationToken);
    }
}
