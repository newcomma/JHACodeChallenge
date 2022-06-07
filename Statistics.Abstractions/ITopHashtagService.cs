namespace Statistics
{
    /// <summary>
    /// Responsible for reporting the top 10 hashtags we have received from the Twitter Sample Stream.
    /// </summary>
    public interface ITopHashtagService
    {
        /// <summary>
        /// Returns the top 10 hashtags we have received from the Twitter Sample Stream.
        /// </summary>
        IEnumerable<HashtagCountDto> GetTopTen();

        /// <summary>
        /// Updates the top-10 list given a collection of hashtag occurances.  
        /// </summary>
        Task AddAsync(IEnumerable<HashtagCountDto> hashtagCounts);
    }
}
