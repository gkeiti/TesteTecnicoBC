using Application.UseCases.Credit.Commands.AddCreditCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace CashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> AddCredit([FromBody] AddCreditCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}
