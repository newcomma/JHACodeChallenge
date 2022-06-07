using Microsoft.Extensions.DependencyInjection;
using Statistics.TweetProcessors;
using TweetProcessing.Abstractions;

namespace Statistics
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds our Tweet streaming services to the DI container.
        /// </summary>
        public static IServiceCollection AddTweetStatistics(this IServiceCollection services)
        {
            services.AddSingleton<ITweetProcessor, LoggingTweetProcessor>();
            services.AddTransient<ITweetProcessor, CountTweetProcessor>();
            services.AddTransient<ITweetProcessor, HashtagTweetProcessor>();

            services.AddSingleton<TweetTotalService>();
            services.AddSingleton<ITopHashtagService, TopHashtagService>();

            return services;
        }
    }
}
