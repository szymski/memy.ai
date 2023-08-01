using Application.Abstractions.Messaging;
using Domain.Stories.Entities;
using MediatR;

namespace Application.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<Story?> {
    public class GetStoryQueryHandler : DbRequestHandler,  IRequestHandler<GetStoryQuery, Story?> {
        public async Task<Story?> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            var story = await Context.Stories.FindAsync(request.Id, cancellationToken);
            return story;
        }

    }
}