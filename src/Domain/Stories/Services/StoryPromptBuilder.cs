using System.Text.RegularExpressions;
using Domain.Stories.Entities;
using Domain.Stories.ValueObjects;

namespace Domain.Stories.Services;

public class StoryPromptBuilder {
    public Prompt BuildPrompt(StoryPreset preset, string[] promptParts, string mainPrompt)
    {
        string[] allParts = promptParts.Concat(new[] { mainPrompt }).ToArray();
        
        string systemMessage = ReplaceWithPrompt(preset.SystemMessage, allParts);
        string userMessage = ReplaceWithPrompt(preset.UserMessage, allParts);

        return new(systemMessage, userMessage);
    }

    internal string ReplaceWithPrompt(string input, string[] parts)
    {
        var result = input;
        var matches = Regex.Matches(input, @"\$([0-9]+)");
        foreach (Match match in matches)
        {
            var index = int.Parse(match.Groups[1].Value);
            if(index >= parts.Length)
                throw new ArgumentException($"Index {index} is out of range of parts array");
            result = result.Replace(match.Value, parts[index]);
        }
        return result;
    }

}