using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetListener : ITweetListener
    {
        private readonly StreamToQueueProcessor streamToChannelProcessor;
        private readonly ITweetQueue tweetQueue;
        private readonly string token;

        public TweetListener(
            StreamToQueueProcessor streamToChannelProcessor, 
            ITweetQueue tweetQueue,
            IOptions<TwitterOptions> options)
        {
            this.streamToChannelProcessor = streamToChannelProcessor;
            this.tweetQueue = tweetQueue;
            this.token = options.Value.Token;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                scheme: "Bearer",
                parameter: token);
           
            using var httpResponse = await httpClient.GetAsync(
                requestUri: "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities",
                completionOption: HttpCompletionOption.ResponseHeadersRead,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            await streamToChannelProcessor.ProcessAsync(stream, tweetQueue, cancellationToken);
        }
    }
}