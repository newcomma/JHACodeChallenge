using Microsoft.Extensions.Logging;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using TweetProcessing.Abstractions;

[assembly: InternalsVisibleTo("TestProject")]

namespace TweetProcessing.ApiV2
{
    /// <summary>
    /// Responsible for placing a stream of bytes onto our queue.
    /// </summary>
    internal class StreamToQueueProcessor
    {
        private readonly ILogger logger;

        public StreamToQueueProcessor(ILogger<StreamToQueueProcessor> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Reads the given <see cref="Stream"/> of bytes and places them onto 
        /// the given <see cref="ITweetQueue"/>.
        /// </summary>
        /// <remarks>
        /// As the bytes are read from the given <see cref="Stream"/> a new tweet is detected
        /// by the pressence of the UTF8 '/n' character. 
        /// </remarks>
        public async Task ProcessAsync(Stream stream, ITweetQueue tweetQueue, CancellationToken cancellationToken = default)
        {
            var pipeReader = PipeReader.Create(stream);

            while (true)
            {
                ReadResult result = await pipeReader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> lineBytes))
                {
                    // writes to our thread-safe queue
                    await tweetQueue.Writer.WriteAsync(lineBytes.ToArray(), cancellationToken);
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
