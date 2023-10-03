using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Credits.Services;
using MediatR;

namespace Application.Credits.Queries;

public record GetUserCreditBalanceQuery : IRequest<decimal> {
    public required int UserId { get; init; }
    
    public class GetUserCreditBalanceQueryHandler(
        CreditService creditService) : IRequestHandler<GetUserCreditBalanceQuery, decimal>, IDbRequestHandler {
        public IAppDbContext Context { get; set; }

        public async Task<decimal> Handle(
            GetUserCreditBalanceQuery request,
            CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(request.UserId);
            return await creditService.GetCreditBalance(user);
        }

    }
}