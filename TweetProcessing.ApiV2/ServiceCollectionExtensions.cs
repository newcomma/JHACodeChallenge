using Microsoft.Extensions.DependencyInjection;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds our Tweet streaming services to the DI container.
        /// </summary>
        public static IServiceCollection AddTweetStreaming(this IServiceCollection services)
        {
            services.AddSingleton<ITweetListener, TweetListener>();
            services.AddTransient<ITweetStreamReader, TweetStreamReader>();
            services.AddTransient<ITweetProcessorPipeline, TweetProcessorPipeline>();

            services.AddSingleton<TweetParser>();
            services.AddSingleton<LinesChannel>();
            services.AddSingleton<StreamToChannelProcessor>();

            // Using 'Transient' to allow scaling horizontally.
            // Each instance of the ITweetStream allows access to the shared tweet queue
            // allowing concurrent processing of tweets.
            services.AddTransient<ITweetStream, TweetStream>();

            return services;
        }
    }
}
