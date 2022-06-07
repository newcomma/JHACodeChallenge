namespace TweetProcessing.Abstractions
{
    /// <summary>
    /// Implementers of this interface can customize the processing "pipeline"
    /// by providing customized logic in the <see cref="ProcessAsync(TweetDto)"/>.
    /// </summary>
    public interface ITweetProcessor
    {
        /// <summary>
        /// Implement this method to provide custom processing logic for each Tweet
        /// received from the Twitter Sample Stream.
        /// </summary>
        Task ProcessAsync(TweetDto tweet);
    }
}
