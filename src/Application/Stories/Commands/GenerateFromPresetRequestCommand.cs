using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Auth.Entities;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.Interfaces;
using Domain.Stories.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Stories.Commands;

public class GenerateFromPresetRequestCommand : IRequest<Story> {
    public required int UserId { get; init; }
    public string PresetId { get; init; }
    public IEnumerable<string> PromptParts { get; init; }
    public string MainPrompt { get; init; }

    public class GenerateFromPresetRequestCommandHandler(
            IMediator mediator,
            IAppDbContext context,
            IStoryPresetStore presetStore,
            StoryPromptBuilder builder)
        : DbRequestHandler, IRequestHandler<GenerateFromPresetRequestCommand, Story> {

        public async Task<Story> Handle(GenerateFromPresetRequestCommand request, CancellationToken cancellationToken)
        {
            // TODO: Disallow generating multiple stories at the same time or add rate limiter

            var preset = presetStore.GetById(request.PresetId)!;
            if (preset is null)
                throw new ArgumentException($"No such preset '{request.PresetId}'");
            Log.Logger.Information("Got request to generate story using preset {@preset}", preset);

            var user = await context.Users.FindAsync(request.UserId);
            
            var prompt = builder.BuildPrompt(preset, request.PromptParts.ToArray(), request.MainPrompt);

            var output = await mediator.Send(new GenerateStoryCommand()
            {
                Model = StoryGeneratorModel.Gpt35Turbo,
                SystemMessage = prompt.SystemMessage,
                UserMessage = prompt.UserMessage,
            }, cancellationToken);

            var story = context.Stories.Add(new()
            {
                User = user,
                Preset = preset.PresetId,
                Model = output.Model,
                Completion = output.Completion,
                PromptParts = request.PromptParts.ToList(),
                MainPrompt = request.MainPrompt,
            }).Entity;

            await context.SaveChangesAsync(cancellationToken);

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