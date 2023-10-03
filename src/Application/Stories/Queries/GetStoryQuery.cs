using Application.Abstractions.Messaging;
using Domain.Stories.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<Story?> {
    public required int UserId { get; init; }
    
    public class GetStoryQueryHandler : DbRequestHandler,  IRequestHandler<GetStoryQuery, Story?> {
        public async Task<Story?> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            var story = await Context.Stories
                .Where(s => s.User.Id == request.UserId)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            return story;
        }

    }
}