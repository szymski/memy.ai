using Application.Common.Interfaces;
using Application.Stories;
using Domain.Credits.Interfaces;
using Domain.Stories.Entities;
using Domain.Stories.Interfaces;
using Infrastructure.Credits.Services;
using Infrastructure.Data;
using Infrastructure.Data.Stories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure;

public static class DependencyInjection {
    private const string SectionDatabase = "Database";
    private const string SectionStoriesPresets = "Stories:Presets";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        #region Database

        var typeStr = config.GetSection(SectionDatabase)?["Type"] ?? "null";
        if (!Enum.TryParse<DbType>(typeStr, true, out var type))
            throw new Exception($"Invalid database type '{typeStr}'");
                
        Log.Logger.Information("Using {0} database provider", type);

        var connectionString = config.GetSection(SectionDatabase)["DefaultConnection"];

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

        #region Story preset store

        services.AddSingleton<IStoryPresetStore, StoryPresetStore>()
            .AddOptions<StoryPresetStore.Options>()
            .Bind(config.GetSection(SectionStoriesPresets))
            .ValidateDataAnnotations();
        
        #endregion

        services.AddScoped<IStoryGenerator, TestStoryGenerator>();

        services.AddScoped<ICreditRepository, CreditRepository>();

        return services;
    }
}
