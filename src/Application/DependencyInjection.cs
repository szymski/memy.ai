using System.Reflection;
using Application.Abstractions.Messaging;
using Application.Common.Messaging.Behaviors;
using Application.Stories.Queries;
using Domain.Credits.Services;
using Domain.Stories.Entities;
using Domain.Stories.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        #region MediatR

        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(configuration => {
                configuration.Lifetime = ServiceLifetime.Scoped;
                configuration.RegisterServicesFromAssembly(assembly);
            })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(DbBehavior<,>));

        // Ensures all IRequestHandler instances are scoped
        services.AddClosedGenericTypes(assembly, typeof(IRequestHandler<,>), ServiceLifetime.Scoped);

        #endregion

        services.AddScoped<StoryPromptBuilder>();

        services.AddScoped<CreditService>();

        return services;
    }

    public static IServiceCollection AddClosedGenericTypes(
        this IServiceCollection services,
        Assembly assembly,
        Type typeToRegister,
        ServiceLifetime serviceLifetime)
    {
        services.Scan(x => x.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeToRegister)
                .Where(t => !t.IsGenericType))
            .AsImplementedInterfaces(t => t.IsGenericType
                                          && t.GetGenericTypeDefinition() == typeToRegister)
            .WithLifetime(serviceLifetime));
        return services;
    }
}