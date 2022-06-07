namespace TweetProcessing.Abstractions
{
    /// <summary>
    /// Represents a pipeline of <see cref="ITweetProcessor"/> instances
    /// to process a given <see cref="TweetDto"/>
    /// </summary>
    public interface ITweetProcessorPipeline
    {
        /// <summary>
        /// Sends the given <see cref="TweetDto"/> through this pipeline of 
        /// <see cref="ITweetProcessor"/> instances
        /// </summary>
        Task ProcessAsync(TweetDto tweet);
    }
}
