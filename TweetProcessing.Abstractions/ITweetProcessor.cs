namespace TweetProcessing.Abstractions
{
    public interface ITweetProcessor
    {
        Task ProcessAsync(TweetDto tweet);
    }
}
