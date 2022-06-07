using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetProcessorPipeline : ITweetProcessorPipeline
    {
        private readonly IEnumerable<ITweetProcessor> tweetProcessors;

        public TweetProcessorPipeline(IEnumerable<ITweetProcessor> tweetProcessors)
        {
            this.tweetProcessors = tweetProcessors;
        }

        public async Task ProcessAsync(TweetDto tweet)
        {
            // Send the given Tweet to each processor in the pipeline
            
            foreach(var tweetProcessor in tweetProcessors)
            {
                await tweetProcessor.ProcessAsync(tweet);
            }
        }
    }
}
