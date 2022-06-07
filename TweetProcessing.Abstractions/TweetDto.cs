namespace TweetProcessing.Abstractions
{
    public class TweetDto
    {
        public string id { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
        public EntitiesDto entities { get; set; } = new EntitiesDto();
    }

    public class EntitiesDto
    {
        public IEnumerable<HashtagDto> hashtags { get; set; } = new List<HashtagDto>();
    }

    public class HashtagDto
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; } = string.Empty;
    }

    public class DataDto
    {
        public TweetDto data { get; set; } = new TweetDto();        
    }
}
