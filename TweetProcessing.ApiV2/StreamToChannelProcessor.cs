using Microsoft.Extensions.Logging;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("TestProject")]

namespace TweetProcessing.ApiV2
{
    internal class StreamToChannelProcessor
    {
        private readonly ILogger logger;

        public StreamToChannelProcessor(ILogger<StreamToChannelProcessor> logger)
        {
            this.logger = logger;
        }

        public async Task ProcessAsync(Stream stream, LinesChannel linesChannel, CancellationToken cancellationToken = default)
        {
            var pipeReader = PipeReader.Create(stream);

            while (true)
            {
                ReadResult result = await pipeReader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> lineBytes))
                {
                    // writes to our thread-safe queue
                    await linesChannel.Writer.WriteAsync(lineBytes.ToArray(), cancellationToken);
                }

                // Tell the PipeReader how much of the buffer has been consumed.
                pipeReader.AdvanceTo(buffer.Start, buffer.End);

                // Stop reading if there's no more data coming.
                if (result.IsCompleted)
                {
                    break;
                }
            }
            // Mark the PipeReader as complete.
            await pipeReader.CompleteAsync();
        }

        bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> lineBytes)
        {
            // Look for 'new line' in the buffer.
            SequencePosition? position = buffer.PositionOf((byte)'\n');
            if (position is null)
            {
                lineBytes = default;
                return false;
            }

            // Skip the line + the \n.
            lineBytes = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }
    }
}
