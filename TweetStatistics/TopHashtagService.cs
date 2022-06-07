using System.Collections.Concurrent;

namespace Statistics
{
    /// <summary>
    /// Thread-safe accessor to the "top 10 hashtags".
    /// </summary>
    public class TopHashtagService : ITopHashtagService
    {
        private ConcurrentDictionary<string, HashtagCountDto> topTenList = new();
        private ConcurrentDictionary<string, int> hashtagCountMap = new();
        private HashtagCountDto lastPlaceHashtag = new HashtagCountDto(0, String.Empty);

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Adds the given <paramref name="hashtagCounts"/> to the total.
        /// This method is thread-safe.
        /// </summary>
        public async Task AddAsync(IEnumerable<HashtagCountDto> hashtagCounts)
        {
            await semaphore.WaitAsync();
            try
            {
                foreach(var hashtagCount in hashtagCounts)
                {
                    int newTotal = UpdateHashtagCount(hashtagCount);
                    UpdateTopTenList(hashtagCount.Hashtag, newTotal);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Updates our in-memory total count for the given <see cref="Hash"/>
        /// </summary>
        /// <param name="hashtagCount"></param>
        private int UpdateHashtagCount(HashtagCountDto hashtagCount)
        {
            return hashtagCountMap.AddOrUpdate(
                key: hashtagCount.Hashtag,
                addValue: hashtagCount.Count,
                updateValueFactory: (key, prev) => prev + hashtagCount.Count);                
        }

        private void UpdateTopTenList(string hashtag, int newTotal)
        {
            // If the given hashtag is already in the Top-10 update it.
            // The Dictionary allows for quick O(1) check.
            if(topTenList.TryGetValue(hashtag, out HashtagCountDto? hashtagCount))
            {
                hashtagCount.Count = newTotal;
                // Recalculate the "Last Place" hashtag ONLY IF we just updated it 
                // because it may not be "Last Place" anymore.
                if (hashtagCount == lastPlaceHashtag)
                {
                    lastPlaceHashtag = topTenList.Values.OrderBy(x => x.Count).First();
                }
            }
            else
            {
                // If Top 10 list doesn't have 10 entries yet, then simply add the hashtag.
                // This will ONLY happen at app startup
                if(topTenList.Count < 10)
                {
                    topTenList.TryAdd(hashtag, new HashtagCountDto(newTotal, hashtag));
                    lastPlaceHashtag = topTenList.Values.OrderBy(x => x.Count).First();
                }
                else
                {
                    /** 
                     * This is the "Hot Path".
                     * As we process each hashtag we want a quick O(1) operation to check
                     * if a hashtag belongs in the "Top 10".  Keeping track of the "Last Place" 
                     * entry of the "Top 10" list gives a quick value to check against to 
                     * avoid unecessary sorting.
                     **/
                    if (newTotal > lastPlaceHashtag.Count)
                    {
                        topTenList.TryAdd(hashtag, new HashtagCountDto(newTotal, hashtag));

                        // remove the last entry (11th)    
                        var orderedHashtags = topTenList.Values.OrderBy(x => x.Count);
                        var last = orderedHashtags.ElementAt(0);
                        topTenList.TryRemove(last.Hashtag, out _);
                        // update the last place
                        lastPlaceHashtag = orderedHashtags.ElementAt(1);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the top 10 Hashtags from our processed Tweets.
        /// </summary>
        public IEnumerable<HashtagCountDto> GetTopTen()
        {
            return topTenList.Values.ToList();
        }
    }
}