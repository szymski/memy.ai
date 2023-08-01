using Microsoft.Build.Framework;

namespace WebApi.Dto; 

public class GenerateStoryDto {
    [Required]
    public string Preset { get; set; }
    [Required]
    public string Prompt { get; set; }
}