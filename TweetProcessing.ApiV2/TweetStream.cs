using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    /// <summary>
    /// Provides a convenient <see cref="IAsyncEnumerable{TweetDto}"/> interface 
    /// to our queue of Tweets, rather than raw bytes. 
    /// </summary>
    internal class TweetStream : ITweetStream
    {
        private readonly ChannelReader<byte[]> channelReader;
        private readonly TweetParser tweetParser;

        public TweetStream(ChannelReader<byte[]> channelReader, TweetParser tweetParser)
        {
            this.channelReader = channelReader;
            this.tweetParser = tweetParser;
        }

        public async IAsyncEnumerable<TweetDto> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach(byte[] line in channelReader.ReadAllAsync(cancellationToken))
            {
                if(tweetParser.TryParse(line, out TweetDto? tweet))
                {
                    yield return tweet;
                }
            };
        }
    }
}
