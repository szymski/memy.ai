using Application.Common;
using Application.Common.Interfaces;
using Domain.Stories.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stories.Queries;

public record GetAllStoriesQuery : IRequest<IEnumerable<Story>> {
    public class GetAllStoriesQueryHandler : IRequestHandler<GetAllStoriesQuery, IEnumerable<Story>> {

        private readonly IAppDbContext _context;

        public GetAllStoriesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Story>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
        {
            var stories = await _context.Stories.ToArrayAsync(cancellationToken: cancellationToken);
            return stories;
        }
    }
}
