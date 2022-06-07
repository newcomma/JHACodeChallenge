using System.Collections.Concurrent;
using TweetProcessing.Abstractions;

namespace Statistics.TweetProcessors
{
    /// <summary>
    /// Records the hashtags of all tweets and keeps a count.
    /// </summary>
    internal class HashtagTweetProcessor : ITweetProcessor
    {
        private readonly ConcurrentDictionary<string, int> hashtagToCountMap = new();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly ITopHashtagService topHashtagService;

        public HashtagTweetProcessor(ITopHashtagService topHashtagService)
        {
            this.topHashtagService = topHashtagService;
        }

        public async Task ProcessAsync(TweetDto tweet)
        {
            await semaphore.WaitAsync();
            try
            {
                UpdateLocalCount(tweet);
                await UpdateGlobalCountAsync();
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Updates our local in-memory count of unique hashtags
        /// </summary>
        private void UpdateLocalCount(TweetDto tweet)
        {
            foreach (var tag in tweet.entities.hashtags.Select(x => x.tag))
            {
                var normalizedHashtag = tag.ToLower();
                hashtagToCountMap.AddOrUpdate(normalizedHashtag, 1, (_, current) => current++);
            }
        }

        /// <summary>
        /// Updates the global <see cref="TopHashtagService"/> which maintains
        /// the total hashtag counts across the application. 
        /// </summary>
        /// <remarks>
        /// The <see cref="TopHashtagService"/> is a shared resource updated by multiple threads
        /// allowing for horizontal scaling.
        /// </remarks>
        private async Task UpdateGlobalCountAsync()
        {
            // Only update the shared resource in batches to reduce contention on shared resource
            // and reduce processing bottelnecks. Allows for better horizontal scaling of compute.
            if (hashtagToCountMap.Count > 10)
            {
                var hashtagCounts = hashtagToCountMap.Select(kvp => new HashtagCountDto(kvp.Value, kvp.Key));
                await topHashtagService.AddAsync(hashtagCounts);
                hashtagToCountMap.Clear();
            }
        }
    }
}
