using Application.Stories.Commands;
using Application.Stories.Queries;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.ValueObjects;
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

        [HttpGet]
        public async ValueTask<ActionResult<List<Story>>> Get()
        {
            var result = await _mediator.Send(new GetAllStoriesQuery());
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<Story>> Get(int id)
        {
            var result = await _mediator.Send(new GetStoryQuery(id));
            return result is not null ? Ok(result) : NotFound();
        }
        
        [HttpPost("generate")]
        public async ValueTask<ActionResult<Story>> Generate()
        {
            var result = await _mediator.Send(new GenerateStoryCommand
            {
                Input = new StoryGenerationInput(StoryGeneratorModel.Gpt35Turbo, "", ""),
            });
            return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
        }
    }
}
