using Domain.Common;

namespace Domain.Stories.Entities;

public class Story : BaseEntity {

    public string Preset { get; set; }

    public string Model { get; set; }

    public string Completion { get; set; }

}