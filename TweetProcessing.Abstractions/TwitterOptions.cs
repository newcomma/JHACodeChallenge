namespace TweetProcessing.Abstractions
{
    public class TwitterOptions
    {
        public const string Twitter = "Twitter";

        /// <summary>
        /// Twitter issued Token
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
