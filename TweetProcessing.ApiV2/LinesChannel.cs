using System.Buffers;
using System.Threading.Channels;

namespace TweetProcessing.ApiV2
{
    internal class LinesChannel
    {
        private Channel<byte[]> channel = Channel.CreateUnbounded<byte[]>();

        public ChannelWriter<byte[]> Writer { get => channel.Writer; }

        public ChannelReader<byte[]> Reader { get => channel.Reader; }
    }
}
