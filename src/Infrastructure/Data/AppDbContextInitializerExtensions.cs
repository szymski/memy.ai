using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class AppDbContextInitializerExtensions {
    public static async Task InitializeDb(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
        await initializer.Initialize();
        await initializer.Seed();
    }
}