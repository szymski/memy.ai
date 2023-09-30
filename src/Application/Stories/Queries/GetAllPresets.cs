using Domain.Stories.Entities;
using Domain.Stories.Interfaces;
using MediatR;

namespace Application.Stories.Queries;

public record GetAllPresetsQuery : IRequest<IEnumerable<StoryPreset>> {
    public class GetAllPresetsQueryHandler : IRequestHandler<GetAllPresetsQuery, IEnumerable<StoryPreset>> {
        private readonly IStoryPresetStore _store;

        public GetAllPresetsQueryHandler(IStoryPresetStore store)
        {
            _store = store;
        }

        public async Task<IEnumerable<StoryPreset>> Handle(
            GetAllPresetsQuery request,
            CancellationToken cancellationToken)
        {
            return _store.GetAll();
        }
    }
}