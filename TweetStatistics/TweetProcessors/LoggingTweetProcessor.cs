using Microsoft.Extensions.Logging;
using TweetProcessing.Abstractions;

namespace Statistics.TweetProcessors
{
    /// <summary>
    /// An <see cref="ITweetProcessor"/> implementation which simply logs Tweet information
    /// </summary>
    internal class LoggingTweetProcessor : ITweetProcessor
    {
        private readonly ILogger<LoggingTweetProcessor> logger;

        public LoggingTweetProcessor(ILogger<LoggingTweetProcessor> logger)
        {
            this.logger = logger;
        }

        public Task ProcessAsync(TweetDto tweet)
        {
            logger.LogInformation($"processing tweet {tweet.text}");
            return Task.CompletedTask;
        }
    }
}
