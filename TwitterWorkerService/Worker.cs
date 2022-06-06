using TweetProcessing.Abstractions;

namespace TwitterWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITweetProcessor _tweetProcessor;
        private readonly ITweetStream _tweetStream;

        public Worker(ILogger<Worker> logger, ITweetProcessor tweetProcessor, ITweetStream tweetStream)
        {
            _logger = logger;
            _tweetProcessor = tweetProcessor;
            _tweetStream = tweetStream;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting Worker Service");

            _ = _tweetProcessor.StartAsync(stoppingToken);

            await foreach(var tweet in _tweetStream.ReadAsync(stoppingToken))
            {
                _logger.LogInformation($"processing tweet {tweet.text}");
            }
        }
    }
}