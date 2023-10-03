using Application.Abstractions.Messaging;
using Domain.Stories.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stories.Queries;

public record GetAllStoriesQuery : IRequest<IEnumerable<Story>> {
    public required int UserId { get; init; }
    
    public class GetAllStoriesQueryHandler : DbRequestHandler, IRequestHandler<GetAllStoriesQuery, IEnumerable<Story>> {
        public async Task<IEnumerable<Story>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            var stories = await Context.Stories
                .Where(s => s.User.Id == request.UserId)
                .Include(s => s.User)
                .ToArrayAsync(cancellationToken: cancellationToken);
            return stories;
        }
    }
}
