using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.Interfaces;
using Domain.Stories.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Stories.Commands;

public class GenerateFromPresetRequestCommand : IRequest<Story> {
    public string PresetId { get; init; }
    public IEnumerable<string> PromptParts { get; init; }
    public string MainPrompt { get; init; }

    public class GenerateFromPresetRequestCommandHandler : DbRequestHandler, IRequestHandler<GenerateFromPresetRequestCommand, Story> {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context;
        private readonly IStoryPresetStore _presetStore;
        private readonly StoryPromptBuilder _builder;

        public GenerateFromPresetRequestCommandHandler(
            IMediator mediator,
            IAppDbContext context,
            IStoryPresetStore presetStore,
            StoryPromptBuilder builder)
        {
            _mediator = mediator;
            _context = context;
            _presetStore = presetStore;
            _builder = builder;
        }

        public async Task<Story> Handle(GenerateFromPresetRequestCommand request, CancellationToken cancellationToken)
        {
            // TODO: Disallow generating multiple stories at the same time or add rate limiter
            
            var preset = _presetStore.GetById(request.PresetId)!;
            if (preset is null)
                throw new ArgumentException($"No such preset '{request.PresetId}'");
            Log.Logger.Information("Got request to generate story using preset {@preset}", preset);

            var prompt = _builder.BuildPrompt(preset, request.PromptParts.ToArray(), request.MainPrompt);

            var output = await _mediator.Send(new GenerateStoryCommand()
            {
                Model = StoryGeneratorModel.Gpt35Turbo,
                SystemMessage = prompt.SystemMessage,
                UserMessage = prompt.UserMessage,
            }, cancellationToken);

            var story = _context.Stories.Add(new()
            {
                Preset = preset.PresetId,
                Model = output.Model,
                Completion = output.Completion,
                PromptParts = request.PromptParts.ToList(),
                MainPrompt = request.MainPrompt,
            }).Entity;

            await _context.SaveChangesAsync(cancellationToken);
            
            return story;
        }
    }

    public class GenerateFromPresetRequestCommandValidator : AbstractValidator<GenerateFromPresetRequestCommand> {
        public GenerateFromPresetRequestCommandValidator(IStoryPresetStore presetStore)
        {
            RuleFor(x => x.PresetId).NotEmpty()
                .Must(id => presetStore.GetById(id) is not null)
                .WithMessage((_, id) => $"No such story preset '{id}'");
            RuleFor(x => x.PromptParts).NotNull()
                .ForEach(x => x.MinimumLength(2));
            RuleFor(x => x.MainPrompt).NotEmpty()
                .MinimumLength(3);
        }
    }
}