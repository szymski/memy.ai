using Domain.Stories.Entities;
using Domain.Stories.ValueObjects;

namespace Application.Stories; 

public interface IStoryGenerator {
    Task<StoryGenerationOutput> Generate(StoryGenerationInput input);
}