using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetStreamReader : ITweetStreamReader
    {
        private readonly ITweetStream tweetStream;
        private readonly ITweetProcessorPipeline pipeline;

        public TweetStreamReader(ITweetStream tweetStream, ITweetProcessorPipeline pipeline)
        {
            this.tweetStream = tweetStream;
            this.pipeline = pipeline;
        }

        /// <summary>
        /// Starts reading from the <see cref="ITweetStream"/> and processing 
        /// incoming <see cref="TweetDto"/>
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
