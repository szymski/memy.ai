using Domain.Stories.Enums;

namespace Domain.Stories.ValueObjects; 

public record StoryGenerationInput(StoryGeneratorModel Model, string SystemMessage, string UserMessage);