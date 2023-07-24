using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ThirdParty.OpenAi; 

public static class OpenAiExtensions {
    public static IServiceCollection AddOpenAi(this IServiceCollection services, OpenAiConfig config)
    {
        // services.Configure<OpenAiConfig>(configuration.GetSection("OpenAi"));
        services.AddSingleton(config);
        services.AddScoped<OpenAiCompletionGenerator>();
        return services;
    }
}