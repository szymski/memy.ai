using Domain.Stories.Entities;
using Domain.Stories.Services;

namespace Domain.UnitTests.Stories.Services;

public class StoryPromptBuilderTests {
    private readonly StoryPromptBuilder _builder;

    public StoryPromptBuilderTests()
    {
        _builder = new StoryPromptBuilder();
    }

    [Fact]
    public void test_single_parameter_replacement()
    {
        // given
        var preset = new StoryPreset
        {
            SystemMessage = "Hello $0",
            UserMessage = "User $0",
        };
        var parts = new string[] {};
        var main = "there";

        // when
        var built = _builder.BuildPrompt(preset, parts, main);

        // then
        Assert.Equal("Hello there", built.SystemMessage);
        Assert.Equal("User there", built.UserMessage);
    }

    [Fact]
    public void test_multiple_parameter_replacement()
    {
        // given
        var preset = new StoryPreset
        {
            SystemMessage = "Hello $2 $0, $1",
            UserMessage = "$0 $1 $2",
        };
        var parts = new string[] { "mate", "have fun" };
        var main = "there";

        // when
        var built = _builder.BuildPrompt(preset, parts, main);

        // then
        Assert.Equal("Hello there mate, have fun", built.SystemMessage);
        Assert.Equal("mate have fun there", built.UserMessage);
    }

    [Fact]
    public void test_throw_on_not_enough_parts()
    {
        var input = "This should $5";
        var parts = new [] { "mate", "have fun" };

        Assert.Throws<ArgumentException>(() => _builder.ReplaceWithPrompt(input, parts));
    }
}