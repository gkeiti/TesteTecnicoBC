using Application.UseCases.Debit.Commands.AddDebitCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public DebitController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddDebitCommand value)
        {
            await _mediatr.Send(value);

            return Ok();
        }
    }
}
