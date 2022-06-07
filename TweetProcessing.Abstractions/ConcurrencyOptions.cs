namespace TweetProcessing.Abstractions
{
    public class ConcurrencyOptions
    {
        public const string Concurrency = "Concurrency";

        /// <summary>
        ///  The number of queue consumers to create to process the tweets concurrently. 
        /// </summary>
        public int QueueConsumerCount { get; set; } = 1;

        /// <summary>
        /// How many Tweets a queue consumer should process before bulk reporting results. 
        /// </summary>
        /// <remarks>
        /// A value of 1 means every tweet will cause an immediate update of shared resources. 
        /// A value of 10 means 10 tweets are processed before updating a shared resource. 
        /// A higher value is a tradeoff between throughput and realtime reporting of data.
        /// </remarks>
        public int BulkTweetCount { get; set; } = 10;
    }
}
