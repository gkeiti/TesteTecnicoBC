using Application.UseCases.Balance.Queries.GetCurrent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public BalanceController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        // GET: api/<Balance>
        [HttpGet]
        public async Task<IActionResult> GetCurrent()
        {
            var result = await _mediatr.Send(new GetCurrentBalanceQuery());

            return Ok(result);
        }

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
        {
            await _mediatr.Send(new GetCurrentBalanceQuery());

            return Ok();
        }
    }
}
