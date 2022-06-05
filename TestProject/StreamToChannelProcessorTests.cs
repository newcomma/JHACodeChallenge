using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using TweetProcessing.ApiV2;

namespace TestProject
{
    public class StreamToChannelProcessorTests
    {
        [Theory]
        [InlineData("1\n", 1)]
        [InlineData("1\n2\n", 2)]
        [InlineData("1\n2\n3\n", 3)]
        [InlineData("1\n2\n3\n4\n", 4)]
        public async Task ProcessAsync_StreamHasLines_ChannelContainsCorrectNumberOfLines(string text, int e)
        {
            // Arrange
            // Convert to stream of UTF8 bytes
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));

            LinesChannel linesChannel = new();
            StreamToChannelProcessor streamToChannelProcessor = new (NullLogger);

            // Act
            await streamToChannelProcessor.ProcessAsync(stream, linesChannel);

            // Assert
            // Assert the Channel has an entry for each line
            var lineCount = text.Count(c => c == '\n');
            Assert.Equal(lineCount, linesChannel.Reader.Count);
        }

        [Fact]
        public async Task ProcessAsync_GivenSampleStream_ChannelContainsCorrectNumberOfLines()
        {
            // Arrange
            LinesChannel linesChannel = new();
            StreamToChannelProcessor streamProcessor = new (NullLogger);

            // Act
            await streamProcessor.ProcessAsync(SampleData.Stream, linesChannel);

            // Assert
            // Assert the channel has the correct number of lines
            var numberOfLines = SampleData.Text.Count(c => c == '\n');
            Assert.Equal(numberOfLines, linesChannel.Reader.Count);
        }


        [Fact]
        public async Task ProcessAsync_GivenSampleStream_ChannelContainsCorrectBytes()
        {
            // Arrange
            LinesChannel linesChannel = new();
            StreamToChannelProcessor streamProcessor = new(NullLogger);

            // Act
            await streamProcessor.ProcessAsync(SampleData.Stream, linesChannel);

            // Assert
            // Assert each entry in the Channel is equal to each line in the SampleData
            var lines = SampleData.Text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach(var line in lines)
            {
                var expected = Encoding.UTF8.GetBytes(line);
                var actual = await linesChannel.Reader.ReadAsync();

                Assert.Equal(expected, actual);
            }            
        }

        /// <summary>
        /// DRY method for creating the <see cref="ILogger"/> for the tests 
        /// </summary>
        private static ILogger<StreamToChannelProcessor> NullLogger
            => new NullLogger<StreamToChannelProcessor>();
    }
}