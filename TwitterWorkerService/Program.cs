using Statistics;
using TweetProcessing.ApiV2;
using TwitterWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddTweetStreaming();
        services.AddTweetStatistics();
    })
    .Build();

await host.RunAsync();
