using Microsoft.Extensions.Logging;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;

namespace TweetProcessing.ApiV2
{
    internal class SampleStreamProcessor
    {
        private readonly Pipe pipe = new Pipe();
        private readonly ILogger logger;

        SampleStreamProcessor(ILogger<SampleStreamProcessor> logger)
        {
            this.logger = logger;
        }

        public async Task ProcessStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            const int minimumBufferSize = 512;

            while (true)
            {
                // Allocate at least 512 bytes from the PipeWriter.
                Memory<byte> memory = pipe.Writer.GetMemory(minimumBufferSize);
                try
                {
                    int bytesRead = await stream.ReadAsync(memory, cancellationToken);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    // Tell the PipeWriter how much was read from the Stream.
                    pipe.Writer.Advance(bytesRead);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception processing Twitter Sample Stream");
                    break;
                }

                // Make the data available to the PipeReader.
                FlushResult result = await pipe.Writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }
            }

            // By completing PipeWriter, tell the PipeReader that there's no more data coming.
            await pipe.Writer.CompleteAsync();
        }

        public async Task ReadPipeAsync()
        {
            while (true)
            {
                ReadResult result = await pipe.Reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (TryReadTweet(ref buffer, out ReadOnlySequence<byte> tweetBytes))
                {
                    ProcessTweet(tweetBytes);
                }

                // Tell the PipeReader how much of the buffer has been consumed.
                pipe.Reader.AdvanceTo(buffer.Start, buffer.End);

                // Stop reading if there's no more data coming.
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // Mark the PipeReader as complete.
            await pipe.Reader.CompleteAsync();
        }

        private void ProcessTweet(ReadOnlySequence<byte> tweetBytes)
        {
            throw new NotImplementedException();
        }

        private readonly byte[] delimiter = Encoding.UTF8.GetBytes("\"data\":");

        bool TryReadTweet(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> tweetBytes)
        {
            var sequenceReader = new SequenceReader<byte>(buffer);
            sequenceReader.TryReadTo(out tweetBytes, delimiter, true);
            
            // Look for 'data' in the buffer.
            buffer.GetPosition()
            SequencePosition? position = buffer.PositionOf((byte)'\n');
          
            if (position == null)
            {
                tweetBytes = default;
                return false;
            }

            // Skip the line + the \n.
            tweetBytes = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }
    }
}
