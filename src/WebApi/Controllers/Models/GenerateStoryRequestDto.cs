using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.Models;

public class GenerateStoryRequestDto {
    [Required]
    public string Preset { get; set; }

    [Required]
    public string Prompt { get; set; }
}