namespace Domain.Stories.ValueObjects;

public record StoryGenerationOutput {
    public required string Model { get; init; }
    public required string SystemMessage { get; init; }
    public required string UserMessage { get; init; }
    public required string Completion { get; init; }

    public required TokenStats TokenStats { get; init; }
    public required TimeSpan TimeTaken { get; init; }
}