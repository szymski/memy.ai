using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Auth.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Queries;

public record GetUserQuery : IRequest<User?> {
    public int UserId { get; set; }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User?>, IDbRequestHandler {
        public IAppDbContext Context { get; set; }

        public async Task<User?> Handle(
            GetUserQuery request,
            CancellationToken cancellationToken)
        {
            return await Context.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);
        }

    }

}