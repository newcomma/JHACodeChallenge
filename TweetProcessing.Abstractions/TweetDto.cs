namespace TweetProcessing.Abstractions
{
    public class TweetDto
    {
        public string id { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }

    public class DataDto
    {
        public TweetDto data { get; set; } = new TweetDto();        
    }
}
