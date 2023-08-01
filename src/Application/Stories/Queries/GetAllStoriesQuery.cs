using Application.Abstractions.Messaging;
using Domain.Stories.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stories.Queries;

public record GetAllStoriesQuery : IRequest<IEnumerable<Story>> {
    public class GetAllStoriesQueryHandler : DbRequestHandler, IRequestHandler<GetAllStoriesQuery, IEnumerable<Story>> {
        public async Task<IEnumerable<Story>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            var stories = await Context.Stories.ToArrayAsync(cancellationToken: cancellationToken);
            return stories;
        }
    }
}
