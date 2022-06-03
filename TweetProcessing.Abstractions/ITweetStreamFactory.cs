namespace TweetProcessing.Abstractions
{
    public interface ITweetStreamFactory
    {
        Task<ITweetStream> Create();
    }
}
