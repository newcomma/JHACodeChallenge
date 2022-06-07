using Statistics;
using TweetProcessing.Abstractions;
using TweetProcessing.ApiV2;
using TwitterWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddTweetStreaming();
        services.AddTweetStatistics();

        services.Configure<ConcurrencyOptions>(
            builder.Configuration.GetSection(ConcurrencyOptions.Concurrency));
        services.Configure<TwitterOptions>(
            builder.Configuration.GetSection(TwitterOptions.Twitter));
    })
    .Build();

await host.RunAsync();
