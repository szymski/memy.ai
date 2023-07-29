using System.Windows.Input;
using Application.Common.Interfaces;
using Domain.Stories.Entities;
using Domain.Stories.ValueObjects;
using FluentValidation;
using MediatR;

namespace Application.Stories.Commands;

public class GenerateStoryCommand : IRequest<Story> {
    public required StoryGenerationInput Input { get; init; }

    public class GenerateStoryCommandHandler : IRequestHandler<GenerateStoryCommand, Story> {
        private readonly IAppDbContext _context;
        private readonly IStoryGenerator _generator;

        public GenerateStoryCommandHandler(
            IAppDbContext context,
            IStoryGenerator generator)
        {
            _context = context;
            _generator = generator;
        }

        public async Task<Story> Handle(GenerateStoryCommand request, CancellationToken cancellationToken)
        {
            var story = await _generator.Generate(request.Input);
            _context.Stories.Add(story);
            await _context.SaveChangesAsync(cancellationToken);
            return story;
        }
    }
    
    public class GenerateStoryCommandValidator : AbstractValidator<GenerateStoryCommand> {
        public GenerateStoryCommandValidator()
        {
            RuleFor(x => x.Input.SystemMessage).NotEmpty();
            RuleFor(x => x.Input.UserMessage).NotEmpty();
        }
    }
}