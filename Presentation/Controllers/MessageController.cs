using Application.Businesses.ReportMessage.Command;
using Application.Businesses.SsoUser.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] InsertMessageRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("[action]/{id:int}")]
        public async Task<IActionResult> Edit([FromRoute]int id,[FromBody] UpdateMessageRequest request)
        {
            try
            {
                request.UpdateMessage.SetMessageId(id);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("[action]/{id:int}")]
        public async Task<IActionResult> Remove([FromRoute] int id, DeleteMessageRequest request)
        {
            try
            {
                request.SetMessageId(id);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("[action]/{id:int}")]
        [Authorize(Roles = "SENIORMANAGER")]
        public async Task<IActionResult> EditForAll([FromRoute] int id, EditForAllMessageRequest request)
        {
            try
            {
                request.EditForAllMessage.SetMessageId(id);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("[action]/{id:int}")]
        [Authorize(Roles = "MIDLEVELMANAGER")]
        public async Task<IActionResult> EditForEmployee([FromRoute] int id, EditForEmployeeMessageRequest request)
        {
            try
            {
                request.EditForEmployeeMessage.SetMessageId(id);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
