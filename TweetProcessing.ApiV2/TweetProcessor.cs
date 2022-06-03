using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    public class TweetProcessor : ITweetProcessor
    {



        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                await ConnectAsync(cancellationToken);
                await ProcessTweetsAsync(cancellationToken);
            }
        }

        private Task ProcessTweetsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}