using Application.UseCases.Balance.Queries.GetCurrent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetCurrent()
        {
            var result = await _mediatr.Send(new GetCurrentBalanceQuery());

            return Ok(result);
        }
    }
}
