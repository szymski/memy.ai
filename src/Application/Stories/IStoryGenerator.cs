using Domain.Stories.Entities;
using Domain.Stories.ValueObjects;

namespace Application.Stories; 

public interface IStoryGenerator {
    Task<Story> Generate(StoryGenerationInput input);
}