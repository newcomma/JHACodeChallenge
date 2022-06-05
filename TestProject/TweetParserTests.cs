using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetProcessing.Abstractions;
using TweetProcessing.ApiV2;

namespace TestProject
{
    public class TweetParserTests
    {
        private const string SAMPLE_TWEET_JSON = "{\"data\":{\"id\":\"1533146071844564992\",\"text\":\"RT @PinoySpyVideos: Gigil na gigil si Roommate, kala nya wala ako. Mahuhuli ba ko o lalabasan muna sya? Full vid. DM lang. More boso vid li…\"}}\r\n";

        [Fact]
        public void TryParse_GivenCorrectJson_ReturnsTrue()
        {
            // Arrange
            string id = "1533146071844564992";
            string json = "{\"data\":{\"id\":\"" + id + "\",\"text\":\"RT @PinoySpyVideos: Gigil na gigil si Roommate, kala nya wala ako. Mahuhuli ba ko o lalabasan muna sya? Full vid. DM lang. More boso vid li…\"}}\r\n";
            TweetParser tweetParser = new(NullLogger);

            // Act
            var result = tweetParser.TryParse(SAMPLE_TWEET_JSON, out TweetDto? tweet);

            // Assert
            Assert.True(result);
            Assert.Equal(id, tweet!.Data!.id);
        }


        /// <summary>
        /// DRY method for creating the <see cref="ILogger"/> for the tests 
        /// </summary>
        private static ILogger<TweetParser> NullLogger => new NullLogger<TweetParser>();
    }
}
