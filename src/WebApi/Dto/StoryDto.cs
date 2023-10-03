using System.ComponentModel.DataAnnotations;
using Domain.Stories.Entities;
using Mapster;

namespace WebApi.Dto; 

public class StoryDto : IBaseEntityDto {
    public int Id { get; set; }
    
    public UserDto User { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public string Preset { get; set; }

    [AdaptMember(nameof(Story.Completion))]
    public string FullStory { get; set; }

    [Required]
    public ICollection<string> PromptParts { get; set; }

    [Required]
    public string MainPrompt { get; set; }
}