using TweetProcessing.Abstractions;

namespace TwitterWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ITweetListener tweetListener;
        private readonly ITweetStreamReader tweetStreamReader;

        public Worker(ILogger<Worker> logger, ITweetListener tweetListener, ITweetStreamReader tweetStreamReader)
        {
            this.logger = logger;
            this.tweetListener = tweetListener;
            this.tweetStreamReader = tweetStreamReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Starting Worker Service");

            _ = tweetListener.StartAsync(stoppingToken);

            await tweetStreamReader.StartAsync(stoppingToken);
        }
    }
}