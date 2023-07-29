using Application.Common.Messaging.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application; 

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssembly(assembly);
        })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        
        return services;
    }
}