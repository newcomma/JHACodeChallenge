using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetParser
    {
        private ILogger<TweetParser> logger;

        public TweetParser(ILogger<TweetParser> logger)
        {
            this.logger = logger;
        }

        internal bool TryParse(Span<byte> bytes, [NotNullWhen(returnValue: true)] out TweetDto? tweetDto)
        {
            try
            {
                string jsonString = Encoding.UTF8.GetString(bytes);
                return TryParse(jsonString, out tweetDto);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, $"Encountered bytes that could not be parsed as a JSON Tweet");
            }
            tweetDto = default;
            return false;
        }


        internal bool TryParse(string json, [NotNullWhen(returnValue: true)] out TweetDto? tweetDto)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    tweetDto = JsonSerializer.Deserialize<DataDto>(json)?.data;
                    if (tweetDto is not null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, $"Encountered UTF8 text that is not a JSON Tweet: text={json}");
            }
            tweetDto = default;
            return false;
        }
    }
}
