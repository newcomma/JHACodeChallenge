namespace Statistics
{
    public interface ITweetTotalService
    {
        /// <summary>
        /// Adds the given <paramref name="count"/> to the total number of Tweets.
        /// </summary>
        Task AddAsync(int count);

        /// <summary>
        /// Returns the total number of Tweets we have received from the Twitter Sample Stream.
        /// </summary>
        int Total { get; }
    }
}
