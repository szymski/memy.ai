using System.Diagnostics;
using Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Models;

namespace WebApi.Auth;

public static class AuthExtensions {
    public static RouteGroupBuilder MapRegisterRoute(
        this RouteGroupBuilder group,
        string path)
    {
        group.MapPost(path, async Task<Results<Ok, ValidationProblem>> (
            [FromBody] RegisterRequestDto dto,
            [FromServices] IMediator mediator) => {
            var result = await mediator.Send(new RegisterUserCommand()
            {
                Email = dto.Email,
                Password = dto.Password,
            });
            
            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });

        return group;
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = new[] { error.Description };
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}