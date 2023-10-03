using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Credits.Entities;
using Domain.Credits.Services;
using MediatR;

namespace Application.Credits.Queries;

public record GetUserCreditEventsQuery : IRequest<IEnumerable<CreditEvent>> {
    public required int UserId { get; init; }
    
    public class GetUserCreditBalanceQueryHandler(
        CreditService creditService) : IRequestHandler<GetUserCreditEventsQuery, IEnumerable<CreditEvent>>, IDbRequestHandler {
        public IAppDbContext Context { get; set; }

        public async Task<IEnumerable<CreditEvent>> Handle(
            GetUserCreditEventsQuery request,
            CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(request.UserId);
            return await creditService.GetEvents(user);
        }

    }
}