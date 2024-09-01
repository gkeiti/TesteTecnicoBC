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

        //// GET: api/<DebitController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<DebitController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<DebitController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddDebitCommand value)
        {
            await _mediatr.Send(value);

            return Ok();
        }

        //// PUT api/<DebitController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DebitController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
