using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Auth.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Auth.Commands;

public record UpdateUserPasswordCommand : IRequest<bool> {
    public required int UserId { get; init; }
    public required string Password { get; init; }

    public class UpdateUserPasswordCommandHandler(
        ILogger<UpdateUserPasswordCommandHandler> logger,
        UserManager<User> userManager) : IRequestHandler<UpdateUserPasswordCommand, bool>, IDbRequestHandler {
        public IAppDbContext Context { get; set; }

        public async Task<bool> Handle(
            UpdateUserPasswordCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating user password for user {UserId}", request.UserId);

            var user = await Context.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, request.Password);

            if (!result.Succeeded)
            {
                logger.LogWarning("Failed to change password for user {UserId}", request.UserId);
                return false;
            }

            return true;
        }
    }
}