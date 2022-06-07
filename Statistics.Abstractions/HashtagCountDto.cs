namespace Statistics
{
    /// <summary>
    /// Represents a hashtag and the count of occurances.
    /// </summary>
    public class HashtagCountDto
    {
        public HashtagCountDto(int count, string hashtag)
        {
            Count = count;
            Hashtag = hashtag;
        }

        /// <summary>
        /// The number of occurances of this hashtag
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The hashtag text
        /// </summary>
        public string Hashtag { get; set; }
    }
}