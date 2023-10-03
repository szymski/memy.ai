using System.Diagnostics;
using Domain.Auth.Entities;
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
            [FromBody] RegisterRequestDto registration,
            [FromServices] IServiceProvider sp) => {
            var userManager = sp.GetRequiredService<UserManager<User>>();

            var userStore = sp.GetRequiredService<IUserStore<User>>();
            var emailStore = (IUserEmailStore<User>)userStore;
            var email = registration.Email;

            var user = new User();
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            await SendConfirmationEmailAsync(user, userManager, email);
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

    // TODO: Implement this
    private static async Task SendConfirmationEmailAsync(
        User user,
        UserManager<User> userManager,
        string email,
        bool isChange = false)
    {
    }
}