using Application.Abstractions.Messaging;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Stories.Commands;

public class GenerateFromPresetRequestCommand : IRequest<Story> {
    public string PresetId { get; init; }
    public string Prompt { get; init; }

    public class GenerateFromPresetRequestCommandHandler : DbRequestHandler, IRequestHandler<GenerateFromPresetRequestCommand, Story> {
        private readonly IMediator _mediator;
        private readonly IStoryPresetStore _presetStore;

        public GenerateFromPresetRequestCommandHandler(
            IMediator mediator,
            IStoryPresetStore presetStore)
        {
            _mediator = mediator;
            _presetStore = presetStore;
        }

        public async Task<Story> Handle(GenerateFromPresetRequestCommand request, CancellationToken cancellationToken)
        {
            var preset = _presetStore.GetById(request.PresetId)!;
            Log.Logger.Warning("Got request to generate story using preset {@preset}", preset);

            var output = await _mediator.Send(new GenerateStoryCommand()
            {
                Model = StoryGeneratorModel.Gpt35Turbo,
                SystemMessage = $"{preset.SystemMessage} - system msg of {request.PresetId}",
                UserMessage = $"{preset.UserMessage} {request.Prompt}"
            }, cancellationToken);

            return new Story()
            {
                Id = 2137,
                Preset = preset.PresetId,
                Model = output.Model,
                Completion = output.Completion,
            };
        }
    }

    public class GenerateFromPresetRequestCommandValidator : AbstractValidator<GenerateFromPresetRequestCommand> {
        public GenerateFromPresetRequestCommandValidator(IStoryPresetStore presetStore)
        {
            RuleFor(x => x.PresetId).NotEmpty()
                .Must(id => presetStore.GetById(id) is not null)
                .WithMessage((_, id) => $"No such story preset '{id}'");
            RuleFor(x => x.Prompt).NotEmpty()
                .MinimumLength(3);
        }
    }
}