using Microsoft.Extensions.DependencyInjection;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    /// <summary>
    /// Factory for creating queue consumers.
    /// </summary>
    internal class TweetQueueConsumerFactory : ITweetQueueConsumerFactory
    {
        private readonly ITweetQueue tweetQueue;
        private readonly TweetParser tweetParser;
        private readonly IServiceProvider sp;

        public TweetQueueConsumerFactory(ITweetQueue tweetQueue, TweetParser tweetParser, IServiceProvider sp)
        {
            this.tweetQueue = tweetQueue;
            this.tweetParser = tweetParser;
            this.sp = sp;
        }

        /// <summary>
        /// Factory method for creating consumers of the <see cref="ITweetQueue"/>.
        /// </summary>
        /// <remarks>
        /// The created <see cref="ITweetQueueConsumer"/> instances process tweet queue
        /// concurrently.
        /// </remarks>
        public IEnumerable<ITweetQueueConsumer> CreateQueueConsumers(int count)
        {
            List<ITweetQueueConsumer> consumers = new();
            for(int i = 0; i < count; i++)
            {
                var consumer = CreateQueueConsumer();
                consumers.Add(consumer);
            }
            return consumers;
        }

        /// <summary>
        /// Factory method for creating a <see cref="ITweetQueueConsumer"/>.
        /// </summary>
        private ITweetQueueConsumer CreateQueueConsumer()
        {
            return new TweetQueueConsumer(
                tweetStream: new TweetStream(
                    channelReader: tweetQueue.Reader,
                    tweetParser: tweetParser),
                pipeline: new TweetProcessorPipeline(
                    tweetProcessors: sp.GetServices<ITweetProcessor>()));
        }
    }
}
