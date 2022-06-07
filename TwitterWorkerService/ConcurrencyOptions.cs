namespace TwitterWorkerService
{
    public class ConcurrencyOptions
    {
        /// <summary>
        ///  The number of <see cref="TweetStreamReader"/> to create
        ///  to process incomming tweets concurrently. 
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// How many Tweets a <see cref="TweetStreamReader"/> should process
        /// individually before bulk reporting results. 
        /// </summary>
        /// <remarks>
        /// A value of 1 means every tweet will cause an immediate update of shared resources. 
        /// A value of 10 means 10 tweets are processed concurrently before updating
        /// a shared resource. 
        /// A higher value is a tradeoff between throughput and realtime reporting of data.
        /// </remarks>
        public int BulkTweetCount { get; set; } = 10;
    }
}
