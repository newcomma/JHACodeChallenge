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
            services.AddSingleton<ITweetQueue, TweetQueue>();
            services.AddSingleton<ITweetQueueConsumerFactory, TweetQueueConsumerFactory>();

            services.AddTransient<ITweetProcessorPipeline, TweetProcessorPipeline>();

            services.AddSingleton<TweetParser>();
            services.AddSingleton<StreamToQueueProcessor>();

            return services;
        }
    }
}
