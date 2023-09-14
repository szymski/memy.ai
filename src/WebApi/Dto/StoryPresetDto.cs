using Domain.Stories.Entities;
using Mapster;

namespace WebApi.Dto; 

public class StoryPresetDto {
    [AdaptMember("PresetId")]
    public string Id { get; set; }
    
    public string Name { get; set; }
}