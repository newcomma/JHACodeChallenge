using System.Threading.Channels;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    /// <summary>
    /// A thread-safe queue containing Tweets received from the 
    /// Twitter Sample Stream. 
    /// </summary>
    /// <remarks>
    /// Each element in the queue is a single Tweet represented
    /// as an array of bytes.
    /// </remarks>
    internal class TweetQueue : ITweetQueue
    {
        private readonly Channel<byte[]> channel = Channel.CreateUnbounded<byte[]>();

        public ChannelWriter<byte[]> Writer { get => channel.Writer; }
        public ChannelReader<byte[]> Reader { get => channel.Reader; }
    }
}
