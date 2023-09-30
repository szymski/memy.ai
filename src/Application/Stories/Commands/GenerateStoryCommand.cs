using System.Windows.Input;
using Application.Common.Interfaces;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Domain.Stories.Services;
using Domain.Stories.ValueObjects;
using FluentValidation;
using MediatR;
using Serilog;

namespace Application.Stories.Commands;

public class GenerateStoryCommand : IRequest<StoryGenerationOutput> {
    public StoryGeneratorModel Model { get; set; }
    public string SystemMessage { get; set; }
    public string UserMessage { get; set; }

    public class GenerateStoryCommandHandler(IStoryGenerator generator) : IRequestHandler<GenerateStoryCommand, StoryGenerationOutput> {

        public async Task<StoryGenerationOutput> Handle(GenerateStoryCommand request, CancellationToken cancellationToken)
        {
            var input = new StoryGenerationInput(request.Model, request.SystemMessage, request.UserMessage);
            Log.Logger.Warning("Got request to generate story {@input}", input);
            var story = await generator.Generate(input);
            return story;
        }
    }

    public class GenerateStoryCommandValidator : AbstractValidator<GenerateStoryCommand> {
        public GenerateStoryCommandValidator()
        {
            RuleFor(x => x.Model)
                .Equal(StoryGeneratorModel.Gpt35Turbo)
                .WithMessage("GPT-4 disabled for development purposes");
            RuleFor(x => x.SystemMessage).NotEmpty();
            RuleFor(x => x.UserMessage).NotEmpty();
        }
    }
}
