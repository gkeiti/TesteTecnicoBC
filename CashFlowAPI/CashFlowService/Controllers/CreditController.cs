using Application.UseCases.Credit.Commands.AddCreditCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CashFlowServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private IMediator _mediator;

        public CreditController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        }

        // POST api/<CreditController>
        [HttpPost]
        public async Task<IActionResult> AddCredit([FromBody] AddCreditCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}
