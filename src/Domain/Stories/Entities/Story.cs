using System.ComponentModel.DataAnnotations.Schema;
using Domain.Auth.Entities;
using Domain.Common;

namespace Domain.Stories.Entities;

public class Story : BaseEntity {
    
    public virtual User User { get; set; }

    public string Preset { get; set; }

    public string Model { get; set; }

    [Column(TypeName = "json")]
    public List<string> PromptParts { get; set; } = new();
    
    public string MainPrompt { get; set; }

    public string Completion { get; set; }

}