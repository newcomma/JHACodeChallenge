using System.Buffers;
using System.Threading.Channels;

namespace TweetProcessing.ApiV2
{
    internal class LinesChannel
    {
        private Channel<ReadOnlySequence<byte>> channel = Channel.CreateUnbounded<ReadOnlySequence<byte>>();

        public ChannelWriter<ReadOnlySequence<byte>> Writer { get => channel.Writer; }

        public ChannelReader<ReadOnlySequence<byte>> Reader { get => channel.Reader; }
    }
}
