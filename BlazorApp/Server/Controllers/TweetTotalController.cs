using Microsoft.AspNetCore.Mvc;
using Statistics;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetTotalController : ControllerBase
    {
        private readonly ILogger<TweetTotalController> logger;
        private readonly ITweetTotalService tweetTotalService;

        public TweetTotalController(ILogger<TweetTotalController> logger, ITweetTotalService tweetTotalService)
        {
            this.logger = logger;
            this.tweetTotalService = tweetTotalService;
        }

        [HttpGet]
        public int Get()
        {
            return tweetTotalService.Total;
        }
    }
}