namespace TweetProcessing.Abstractions
{
    /// <summary>
    /// Factory for creating Twitter queue consumers
    /// </summary>
    public interface ITweetQueueConsumerFactory
    {
        /// <summary>
        /// Factory method for creating consumers of the <see cref="ITweetQueue"/>.
        /// </summary>
        /// <remarks>
        /// The created <see cref="ITweetQueueConsumer"/> instances consume the 
        /// tweet queue concurrently.
        /// </remarks>
        public IEnumerable<ITweetQueueConsumer> CreateQueueConsumers(int count);
    }
}
