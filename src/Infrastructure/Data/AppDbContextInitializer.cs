using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class AppDbContextInitializer {
    private readonly ILogger<AppDbContextInitializer> _logger;
    private readonly AppDbContext _context;

    public AppDbContextInitializer(
        ILogger<AppDbContextInitializer> logger,
        AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Initialize()
    {
        _logger.LogInformation("Initializing database...");

        _logger.LogInformation("Recreating the database...");
        
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        await _context.Database.MigrateAsync();
    }

    public async Task Seed()
    {
        _logger.LogInformation("Seeding database...");

        if (await _context.Stories.AnyAsync() == false)
        {
            _context.Stories.Add(new()
            {
                Model = StoryGeneratorModel.Gpt35Turbo.ToString(),
                Preset = "Story 1",
            });
            _context.Stories.Add(new()
            {
                Model = StoryGeneratorModel.Gpt4.ToString(),
                Preset = "Story 2",
            });
            _context.Stories.Add(new()
            {
                Model = "Some random model",
                Preset = "Preset of a story of id 3",
            });
        }

        await _context.SaveChangesAsync();
    }
}