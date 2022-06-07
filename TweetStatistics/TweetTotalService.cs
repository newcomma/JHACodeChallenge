namespace Statistics
{
    /// <summary>
    /// Thread-safe accessor to the 'total' number of Tweets.
    /// </summary>
    public class TweetTotalService : ITweetTotalService
    {
        private int total;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Adds the given <paramref name="count"/> to the total number of Tweets.
        /// This method is thread-safe.
        /// </summary>
        public async Task AddAsync(int count)
        {
            await semaphore.WaitAsync();
            try
            {
                Interlocked.Add(ref total, count);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Returns the total number of Tweets we have received
        /// from the Twitter Sample Stream.
        /// </summary>
        public int Total { get => total; }
    }
}