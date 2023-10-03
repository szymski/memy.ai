using System.Data.Common;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Auth.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Auth.Commands;

public record UpdateUserEmailCommand : IRequest<bool> {
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public IDbContextTransaction? Transaction { get; init; }

    public class UpdateUserEmailCommandHandler(
        ILogger<UpdateUserEmailCommandHandler> logger,
        UserManager<User> userManager) : IRequestHandler<UpdateUserEmailCommand, bool>, IDbRequestHandler {
        public IAppDbContext Context { get; set; }

        public async Task<bool> Handle(
            UpdateUserEmailCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating user email for user {UserId} to {Email}", request.UserId, request.Email);

            var user = await Context.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

            var transaction = request.Transaction ?? Context.BeginTransaction();

            try
            {
                var result1 = await userManager.SetUserNameAsync(user, request.Email);
                if (!result1.Succeeded)
                    throw new Exception($"Failed to change username to {request.Email}");

                var result2 = await userManager.SetEmailAsync(user, request.Email);
                if (!result2.Succeeded)
                    throw new Exception($"Failed to change email to {request.Email}");

                if (request.Transaction is null)
                    await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogWarning("Failed to change email for user {UserId}: {Msg}", request.UserId, e.Message);
                return false;
            }
            
            if(request.Transaction is null)
                await transaction.DisposeAsync();

            // TODO: Email verification

            return true;
        }
    }

    public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand> {
        public UpdateUserEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}