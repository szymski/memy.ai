using Application.Common.Interfaces;
using Domain.Stories.Entities;
using MediatR;
using Serilog;

namespace Application.Stories.Queries;

public record GetStoryQuery(int Id) : IRequest<Story?>;

public class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, Story?> {

    private readonly IAppDbContext _context;

    public GetStoryQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Story?> Handle(GetStoryQuery request, CancellationToken cancellationToken)
    {
        Log.Logger.Information("Hello from {0}, querying id {1}", this,
                               request.Id);
        var story = await _context.Stories.FindAsync(request.Id, cancellationToken);
        return story;
    }
}