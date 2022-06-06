using Microsoft.Extensions.Logging;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetStream : ITweetStream
    {
        private readonly LinesChannel channel;
        private readonly TweetParser tweetParser;

        public TweetStream(LinesChannel channel, TweetParser tweetParser, ILogger<TweetStream> logger)
        {
            this.channel = channel;
            this.tweetParser = tweetParser;
        }

        public async IAsyncEnumerable<TweetDto> ReadAsync(CancellationToken cancellationToken)
        {
            await foreach(byte[] line in channel.Reader.ReadAllAsync(cancellationToken))
            {
                if(tweetParser.TryParse(line, out TweetDto? tweet))
                {
                    yield return tweet;
                }
            };
        }
    }
}
