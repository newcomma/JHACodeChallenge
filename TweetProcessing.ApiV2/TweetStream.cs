using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Text.Json;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetStream : ITweetStream
    {
        private readonly LinesChannel channel;
        private readonly ILogger<TweetStream> logger;

        public TweetStream(LinesChannel channel, ILogger<TweetStream> logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        public async IAsyncEnumerable<TweetDto> ReadTweetsAsync(CancellationToken cancellationToken)
        {
            await foreach(byte[] line in channel.Reader.ReadAllAsync(cancellationToken))
            {
                TweetDto? tweet = null;
                try
                {
                    tweet = JsonSerializer.Deserialize<TweetDto>(line);
                }
                catch (JsonException jsonException)
                {
                    logger.LogWarning(jsonException, "Encountered Json that is not a Tweet");
                }
                if (tweet is not null)
                {
                    yield return tweet;
                }
            };
        }
    }
}
