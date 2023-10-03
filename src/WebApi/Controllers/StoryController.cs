using Application.Stories.Commands;
using Application.Stories.Queries;
using Domain.Stories.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Auth.Services;
using WebApi.Controllers.Models;
using WebApi.Dto;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StoryController : ControllerBase {
        private readonly IMediator _mediator;
        private readonly CurrentUserAccessor _currentUserAccessor;

        public StoryController(
            IMediator mediator,
            CurrentUserAccessor currentUserAccessor
        )
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet("user")]
        [AllowAnonymous]
        public async ValueTask<ActionResult> GetUser()
        {
            if(!_currentUserAccessor.IsAuthenticated)
                return Ok("Not authenticated");

            return Ok(_currentUserAccessor.User);
        }

        /// <summary>
        /// Gets all stories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async ValueTask<ActionResult<List<StoryDto>>> Get()
        {
            var result = await _mediator.Send(new GetAllStoriesQuery()
            {
                UserId = _currentUserAccessor.User.Id,
            });
            return Ok(result.Adapt<List<StoryDto>>());
        }

        /// <summary>
        /// Gets story by id.
        /// </summary>
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<StoryDto>> GetById(
            int id
        )
        {
            var result = await _mediator.Send(new GetStoryQuery(id)
            {
                UserId = _currentUserAccessor.User.Id,
            });
            if (result is null) return NotFound();
            return Ok(result.Adapt<StoryDto>());
        }

        /// <summary>
        /// Generates a new story from a preset.
        /// </summary>
        /// <param name="requestDto"></param>
        [HttpPost("generate")]
        [Consumes(typeof(GenerateStoryRequestDto), "application/json")]
        [AllowAnonymous]
        public async ValueTask<ActionResult<Story>> Generate(
            GenerateStoryRequestDto requestDto
        )
        {
            Log.Logger.Warning("Received generate story request: {@requestDto}", requestDto);

            var result = await _mediator.Send(new GenerateFromPresetRequestCommand()
            {
                PresetId = requestDto.Preset,
                PromptParts = requestDto.PromptParts,
                MainPrompt = requestDto.MainPrompt,
            });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        /// <summary>
        /// Returns all available presets for story generation.
        /// </summary>
        [HttpGet("presets")]
        [AllowAnonymous]
        public async ValueTask<ActionResult<IEnumerable<StoryPresetDto>>> GetPresets()
        {
            var result = await _mediator.Send(new GetAllPresetsQuery());
            return Ok(result.Adapt<IEnumerable<StoryPresetDto>>());
        }
    }
}