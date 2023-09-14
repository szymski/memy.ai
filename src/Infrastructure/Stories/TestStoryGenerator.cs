using Application.Stories;
using Domain.Stories.ValueObjects;

namespace Infrastructure;

public class TestStoryGenerator : IStoryGenerator {

    public async Task<StoryGenerationOutput> Generate(StoryGenerationInput input)
    {
        return new()
        {
            Model = input.Model.ToString(),
            UserMessage = input.UserMessage,
            SystemMessage = input.SystemMessage,
            Completion = $"This is a test story generated from {input.UserMessage} user message.",
            TokenStats = new TokenStats(21, 37),
            TimeTaken = TimeSpan.FromSeconds(5),
        };
    }
}