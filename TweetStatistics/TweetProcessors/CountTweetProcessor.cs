using TweetProcessing.Abstractions;

namespace Statistics.TweetProcessors
{
    /// <summary>
    /// Counts the tweets as they come in and reports the count 
    /// to the <see cref="TweetTotalService"/>
    /// </summary>
    internal class CountTweetProcessor : ITweetProcessor
    {
        private int count = 0;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly TweetTotalService tweetTotal;

        public CountTweetProcessor(TweetTotalService tweetTotal)
        {
            this.tweetTotal = tweetTotal;
        }

        public async Task ProcessAsync(TweetDto tweet)
        {
            await _semaphore.WaitAsync();
            try
            {
                count++;

                // Bulk update after processing more than one tweet
                // to relieve contention on the shared-resource.
                if (count >= 10)
                {
                    await tweetTotal.AddAsync(count);
                    count = 0;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
