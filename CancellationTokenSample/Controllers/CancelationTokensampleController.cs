using Microsoft.AspNetCore.Mvc;

namespace CancellationTokenSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CancelationTokensampleController : ControllerBase
    {
     
        private readonly ILogger<CancelationTokensampleController> _logger;

        public CancelationTokensampleController(ILogger<CancelationTokensampleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("no-cancelation")]
        public async Task<ActionResult> Get()
        {

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                Console.WriteLine("Running Mode!");
            }

            return Ok();

        }
    }
}