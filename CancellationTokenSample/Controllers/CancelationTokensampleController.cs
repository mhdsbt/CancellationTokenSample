using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace CancellationTokenSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CancelationTokensampleController : ControllerBase
    {

        private IApplicationDbContext _context;
        public CancelationTokensampleController(IApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet("no-cancelation")]
        public async Task<ActionResult> Get()
        {
            await DoSomthing(new Random().Next(1, 1000));
            return Ok();
        }

        [HttpGet("with-Cancellation-source")]
        public async Task<ActionResult> GetWithCancellationSource()
        {
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));



            await DoSomthing(new Random().Next(1, 1000), cancellationTokenSource.Token);

            return Ok();
        }

        [HttpGet("with-Requested-source")]
        public async Task<ActionResult> GetWithRequestedSource(CancellationToken cancellationToken)
        {

            await DoSomthing(new Random().Next(1, 1000), cancellationToken);

            return Ok();
        }



        [HttpGet("Get-With-EF-QueryCancelation")]
        public async Task<ActionResult> GetWithEFQueryCancelation()
        {
            var result = await _context.GetProductsWithDelayAsync(HttpContext.RequestAborted);

            return Ok(result);
        }


        private async Task DoSomthing(int taskId, CancellationToken cancellationToken = default)
        {
            Console.Clear();

            try
            {

                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                    Console.WriteLine($"Running Mode! TaskId: {taskId}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Task Cancelled hehe!");
            }
        }
    }
}