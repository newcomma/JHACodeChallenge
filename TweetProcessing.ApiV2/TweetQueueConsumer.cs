using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetQueueConsumer : ITweetQueueConsumer
    {
        private readonly ITweetStream tweetStream;
        private readonly ITweetProcessorPipeline pipeline;

        public TweetQueueConsumer(ITweetStream tweetStream, ITweetProcessorPipeline pipeline)
        {
            this.tweetStream = tweetStream;
            this.pipeline = pipeline;
        }

        /// <summary>
        /// Starts reading from the <see cref="ITweetQueue"/> and processing 
        /// incoming <see cref="TweetDto"/> through the "pipline"
        /// </summary>
        public async Task StartAsync(CancellationToken stoppingToken)
        {
            await foreach (var tweet in tweetStream.ReadAsync(stoppingToken))
            {
                await pipeline.ProcessAsync(tweet);
            }
        }
    }
}
