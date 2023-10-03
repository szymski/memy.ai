using Domain.Auth.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands;

public record RegisterUserCommand : IRequest<IdentityResult> {
    public required string Email { get; init; }
    public required string Password { get; init; }

    public class RegisterUserCommandHandler(
        UserManager<User> userManager,
        IUserStore<User> userStore
    ) : IRequestHandler<RegisterUserCommand, IdentityResult> {
        public async Task<IdentityResult> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var emailStore = (IUserEmailStore<User>)userStore;
            var email = request.Email;

            var user = new User();
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return result!;
            }

            await SendConfirmationEmailAsync(user, userManager, email);

            return result!;
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
}