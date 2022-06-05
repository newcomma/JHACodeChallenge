namespace TweetProcessing.Abstractions
{
    public class TweetDto
    {
        public DataDto? Data { get; set; }
        
    }

    public class DataDto
    {
        public string id { get; set; } = string.Empty;
    }
}
