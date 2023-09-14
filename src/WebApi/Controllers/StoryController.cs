using Application.Stories.Commands;
using Application.Stories.Queries;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Controllers.Models;
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
        public async ValueTask<ActionResult<List<StoryDto>>> Get()
        {
            var result = await _mediator.Send(new GetAllStoriesQuery());
            return Ok(result.Adapt<List<StoryDto>>());
        }

        /// <summary>
        /// Gets story by id.
        /// </summary>
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<StoryDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetStoryQuery(id));
            if (result is null) return NotFound();
            return Ok(result.Adapt<StoryDto>());
        }

        /// <summary>
        /// Generates a new story from a preset.
        /// </summary>
        /// <param name="requestDto"></param>
        [HttpPost("generate")]
        [Consumes(typeof(GenerateStoryRequestDto), "application/json")]
        public async ValueTask<ActionResult<Story>> Generate(GenerateStoryRequestDto requestDto)
        {
            Log.Logger.Warning("Received generate story request: {@requestDto}", requestDto);

            var result = await _mediator.Send(new GenerateFromPresetRequestCommand()
            {
                PresetId = requestDto.Preset,
                Prompt = requestDto.Prompt,
            });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        /// <summary>
        /// Returns all available presets for story generation.
        /// </summary>
        [HttpGet("presets")]
        public async ValueTask<ActionResult<IEnumerable<StoryPresetDto>>> GetPresets()
        {
            var result = await _mediator.Send(new GetAllPresetsQuery());
            return Ok(result.Adapt<IEnumerable<StoryPresetDto>>());
        }
    }
}