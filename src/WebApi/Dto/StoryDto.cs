using Domain.Stories.Entities;
using Mapster;

namespace WebApi.Dto; 

public class StoryDto : IBaseEntityDto {
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public string Preset { get; set; }

    [AdaptMember(nameof(Story.Completion))]
    public string FullStory { get; set; }
}