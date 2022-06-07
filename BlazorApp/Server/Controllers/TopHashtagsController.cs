using Microsoft.AspNetCore.Mvc;
using Statistics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TopHashtagsController : ControllerBase
    {
        private readonly ILogger<TopHashtagsController> logger;
        private readonly ITopHashtagService topHashtagService;

        public TopHashtagsController(ILogger<TopHashtagsController> logger, ITopHashtagService topHashtagService)
        {
            this.logger = logger;
            this.topHashtagService = topHashtagService;
        }

        [HttpGet]
        public IEnumerable<HashtagCountDto> Get()
        {
            return topHashtagService.GetTopTen();
        }
    }
}
