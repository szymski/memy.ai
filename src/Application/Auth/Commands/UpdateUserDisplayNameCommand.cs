using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Auth.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands;

public record UpdateUserDisplayNameCommand : IRequest<User> {
    public required int UserId { get; init; }
    public string? DisplayName { get; init; }


    public class UpdateUserDisplayNameCommandHandler : IRequestHandler<UpdateUserDisplayNameCommand, User>, IDbRequestHandler {
        public IAppDbContext Context { get; set; }

        public async Task<User> Handle(
            UpdateUserDisplayNameCommand request,
            CancellationToken cancellationToken)
        {
            var user = await Context.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);
            if (user.DisplayName != request.DisplayName)
            {
                user.DisplayName = request.DisplayName;
                await Context.SaveChangesAsync(cancellationToken);
            }
            return user;
        }
    }

    public class UpdateUserDisplayNameCommandValidator : AbstractValidator<UpdateUserDisplayNameCommand> {
        public UpdateUserDisplayNameCommandValidator()
        {
            RuleFor(x => x.DisplayName)
                .MaximumLength(30);
        }
    }
}