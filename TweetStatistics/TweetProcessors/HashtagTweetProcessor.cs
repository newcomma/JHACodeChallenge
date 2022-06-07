using System.Collections.Concurrent;
using TweetProcessing.Abstractions;

namespace Statistics.TweetProcessors
{
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
        /// Updates our local 
        /// </summary>
        private void UpdateLocalCount(TweetDto tweet)
        {
            foreach (var hashtag in tweet.entities.hashtags.Select(x => x.tag))
            {
                hashtagToCountMap.AddOrUpdate(hashtag, 1, (_, current) => current++);
            }
        }

        private async Task UpdateGlobalCountAsync()
        {
            // Only update the shared resource in batches to reduce contention on shared resource
            // and reduce processing bottelneck. Allows for better horizontal scaling of compute.
            if (hashtagToCountMap.Count > 10)
            {
                var hashtagCounts = hashtagToCountMap.Select(kvp => new HashtagCountDto(kvp.Value, kvp.Key));
                await topHashtagService.AddAsync(hashtagCounts);
                hashtagToCountMap.Clear();
            }
        }
    }
}
