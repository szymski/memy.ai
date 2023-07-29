using Application.Common.Interfaces;
using Application.Stories;
using Domain.Stories.Entities;
using Domain.Stories.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        #region Database

        var typeStr = config.GetSection("Database")?["Type"] ?? "null";
        if (!Enum.TryParse<DbType>(typeStr, true, out var type))
            throw new Exception($"Invalid database type '{typeStr}'");
                
        Log.Logger.Information("Using {0} database provider", type);

        var connectionString = config.GetSection("Database")["DefaultConnection"];

        services.AddDbContext<AppDbContext>((provider, builder) => {
            switch (type)
            {
                case DbType.Sqlite:
                    builder.UseSqlite(connectionString);
                    break;
                case DbType.Postgres:
                default:
                    throw new NotImplementedException("Postgres not implemented");
            }
        });
        
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<AppDbContextInitializer>();

        #endregion

        services.AddScoped<IStoryGenerator, StoryGenerator>();

        return services;
    }
}

public class StoryGenerator : IStoryGenerator {

    public async Task<Story> Generate(StoryGenerationInput input)
    {
        return new()
        {
            Model = input.Model.ToString(),
            Preset = "GeneratedPreset",
        };
    }
}