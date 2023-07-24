using Application.Stories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase {
        private readonly IMediator _mediator;

        public StoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetStoryQuery(id));
            return result is not null ? Ok(result) : NotFound();
        }
    }
}
