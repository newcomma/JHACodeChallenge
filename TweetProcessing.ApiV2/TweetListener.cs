using System.Net.Http.Headers;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetListener : ITweetListener
    {
        private readonly StreamToChannelProcessor streamToChannelProcessor;
        private readonly LinesChannel linesChannel;

        public TweetListener(StreamToChannelProcessor streamToChannelProcessor, LinesChannel linesChannel)
        {
            this.streamToChannelProcessor = streamToChannelProcessor;
            this.linesChannel = linesChannel;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                scheme: "Bearer",
                parameter: "AAAAAAAAAAAAAAAAAAAAAKkEdQEAAAAABy0zBygPRP5JbigYnzH7L%2BW71R8%3DoxJimqeYNvNZbBJG0A7XN1W11VlafisS6VSDIScxrYT6hKimIa");
           
            using var httpResponse = await httpClient.GetAsync(
                requestUri: "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities",
                completionOption: HttpCompletionOption.ResponseHeadersRead,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            await streamToChannelProcessor.ProcessAsync(stream, linesChannel, cancellationToken);
        }
    }
}