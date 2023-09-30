using Domain.Auth.Entities;
using Domain.Stories.Entities;
using Domain.Stories.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class AppDbContextInitializer(
    ILogger<AppDbContextInitializer> logger,
    AppDbContext context,
    UserManager<User> userManager) {

    public async Task Initialize()
    {
        logger.LogInformation("Initializing database...");

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
    }

    public async Task Seed()
    {
        logger.LogInformation("Seeding database...");

        #region Users

        var user1 = new User()
        {
            Email = "test@test.pl",
        };
        {
            var result = await userManager.CreateAsync(user1, "test1234");
            if (!result.Succeeded)
                throw new($"Failed to create user: {result.Errors.Select(x => x.Description).Aggregate((a, b) => $"{a}, {b}")}");
        }

        var user2 = new User()
        {
            Email = "a@a.pl",
        };
        {
            var result = await userManager.CreateAsync(user2, "test1234");
            if (!result.Succeeded)
                throw new($"Failed to create user: {result.Errors.Select(x => x.Description).Aggregate((a, b) => $"{a}, {b}")}");
        }

        #endregion

        #region Stories

        if (await context.Stories.AnyAsync() == false)
        {
            context.Stories.Add(new()
            {
                User = user1,
                Model = StoryGeneratorModel.Gpt35Turbo.ToString(),
                Preset = "Story 1",
                Completion = "Completion 1",
                MainPrompt = "prompcik 1",
            });
            context.Stories.Add(new()
            {
                User = user2,
                Model = StoryGeneratorModel.Gpt4.ToString(),
                Preset = "Story 2",
                Completion = "Sratata",
                MainPrompt = "drugi",
            });
            context.Stories.Add(new()
            {
                User = user1,
                Model = "Some random model",
                Preset = "Preset of a story of id 3",
                Completion = "Historyjka",
                MainPrompt = "trzy",
            });
        }

        #endregion

        await context.SaveChangesAsync();
    }
}