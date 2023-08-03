using Domain.Stories.Entities;

namespace Domain.Stories.Interfaces; 

public interface IStoryPresetStore {
    IEnumerable<StoryPreset> GetAll();
    StoryPreset? GetById(string id);
}