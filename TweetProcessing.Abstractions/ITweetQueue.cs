using System.Threading.Channels;

namespace TweetProcessing.Abstractions
{
    public interface ITweetQueue
    {
        ChannelWriter<byte[]> Writer { get; }
        ChannelReader<byte[]> Reader { get; }
    }
}
