using Application.UseCases.Debit.Commands.AddDebitCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CashFlowServiceAPI.Controllers
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

        // POST api/<DebitController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddDebitCommand value)
        {
            await _mediatr.Send(value);

            return Ok();
        }
    }
}
