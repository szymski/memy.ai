using Domain.Common;

namespace Domain.Stories.Entities; 

public class StoryPreset : BaseEntity {
    
    public string PresetId { get; set; }
    
    public string Name { get; set; }
    
    public string SystemMessage { get; set; }
    
    public string UserMessage { get; set; }
    
        
}