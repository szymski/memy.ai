using Application.Stories.Commands;
using Application.Stories.Queries;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Dto;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase {
        private readonly IMediator _mediator;

        public StoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all stories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async ValueTask<ActionResult<List<Story>>> Get()
        {
            var result = await _mediator.Send(new GetAllStoriesQuery());
            return Ok(result);
        }

        private static int i = 0;
        
        /// <summary>
        /// Gets story by id.
        /// </summary>
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<Story>> Get(int id)
        {
            var result = await _mediator.Send(new GetStoryQuery(id));
            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Generates a new story from a preset.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("generate")]
        [Consumes(typeof(GenerateStoryDto), "application/json")]
        public async ValueTask<ActionResult<Story>> Generate(GenerateStoryDto dto)
        {
            Log.Logger.Warning("Received generate story request: {@dto}", dto);

            var result = await _mediator.Send(new GenerateFromPresetRequestCommand()
            {
                PresetId = dto.Preset,
                Prompt = dto.Prompt,
            });
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}