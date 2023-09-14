using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Domain.Stories.Entities;
using Domain.Stories.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Data.Stories;

public class StoryPresetStore : IStoryPresetStore {

    public class Options {
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_\\\-/_]+\.json")]
        public string Filename { get; set; }
    }

    private readonly Options _options;

    public StoryPresetStore(IOptions<Options> options)
    {
        _options = options.Value;
        Log.Logger.Information("Loading story presets from {0}", _options.Filename);

        var path = Path.Combine(Directory.GetCurrentDirectory(), _options.Filename);
        if (!File.Exists(path))
        {
            Log.Logger.Warning("File {0} does not exist, creating...", path);

            _presets.Add(new()
            {
                PresetId = "fanatyk",
                Name = "Mój stary to fanatyk",
                SystemMessage = "",
                UserMessage = "Mój stary to fanatyk ",
            });

            File.WriteAllText(path, JsonSerializer.Serialize(_presets, new JsonSerializerOptions { WriteIndented = true }));
        }

        _presets = JsonSerializer.Deserialize<List<StoryPreset>>(File.ReadAllText(path))!;

        Log.Logger.Information("Loaded {0} story presets: {1}",
            _presets.Count, _presets.Select(p => p.PresetId));
    }

    private readonly List<StoryPreset> _presets = new();

    public IEnumerable<StoryPreset> GetAll()
    {
        return _presets;
    }

    public StoryPreset? GetById(string id)
    {
        return _presets.FirstOrDefault(x => x.PresetId == id)!;
        // ?? throw new KeyNotFoundException($"No such story preset id '{id}'");
    }
}