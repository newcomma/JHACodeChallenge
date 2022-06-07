using Microsoft.Extensions.Options;
using TweetProcessing.Abstractions;

namespace TwitterWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ConcurrencyOptions options;
        private readonly ITweetListener tweetListener;
        private readonly ITweetQueueConsumerFactory factory;


        public Worker(
            ILogger<Worker> logger, 
            ITweetListener tweetListener,
            ITweetQueueConsumerFactory factory,
            IOptions<ConcurrencyOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;

            this.tweetListener = tweetListener;
            this.factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Starting Worker Service");

            _ = tweetListener.StartAsync(stoppingToken);

            // create the configured number of queue consumers
            var concurrentConsumers = factory.CreateQueueConsumers(options.QueueConsumerCount);
            // Start the queue consumers
            await Task.WhenAll(concurrentConsumers.Select(x => x.StartAsync(stoppingToken)));
        }
    }
}