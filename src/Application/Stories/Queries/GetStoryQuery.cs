using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.Stories.Entities;
using MediatR;
using Serilog;

namespace Application.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<Story?> {
    public class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, Story?>, IDbRequestHandler {

        public IAppDbContext Context { get; set; }

        public async Task<Story?> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            var story = await Context.Stories.FindAsync(request.Id, cancellationToken);
            return story;
        }

    }
}