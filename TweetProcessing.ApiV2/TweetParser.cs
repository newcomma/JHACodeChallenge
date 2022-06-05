using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        internal bool TryParse(string json, out TweetDto? tweetDto)
        {
            try
            {

                tweetDto = JsonSerializer.Deserialize<TweetDto>(json);
                return true;
            }
            catch (JsonException jsonException)
            {
                logger.LogWarning(jsonException, "Encountered Json that is not a Tweet");
            }
            tweetDto = default;
            return false;
        }
    }
}
