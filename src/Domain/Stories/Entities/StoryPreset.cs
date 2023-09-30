using System.Runtime.CompilerServices;

namespace Domain.Stories.Entities; 

// TODO: Keep this in DB and uncomment BaseEntity
public class StoryPreset /*: BaseEntity*/ {
    
    public string PresetId { get; set; }
    
    public string Name { get; set; }
    
    public required string SystemMessage { get; set; }
    
    public required string UserMessage { get; set; }
    
    // [Column(TypeName = "json")]
    // public List<string> PromptParts { get; set; }
    
    // public string MainPrompt { get; set; }
}