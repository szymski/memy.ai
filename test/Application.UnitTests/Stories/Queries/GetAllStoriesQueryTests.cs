using Application.Common.Interfaces;
using Application.Stories.Queries;
using Application.UnitTests.Helpers;
using Domain.Stories.Entities;

namespace Application.UnitTests.Stories.Queries; 

public class GetAllStoriesQueryTests {
    [Fact]
    public async Task Handle_Should_Return_Stories()
    {
        // given

        var stories = new List<Story>()
        {
            new()
            {
                Id = 3,
                Preset = "preset3",
                Model = "mdl3",
            },
            new()
            {
                Id = 5,
                Preset = "test_preset",
                Model = "mdl",
            },
        };
        var dbSetMock = DbHelper.GetMockDbSet(stories);
        var contextMock = new Mock<IAppDbContext>();
        contextMock.Setup(c => c.Stories)
            .Returns(() => dbSetMock.Object);

        var command = new GetAllStoriesQuery();
        var handler = new GetAllStoriesQuery.GetAllStoriesQueryHandler();
        handler.Context = contextMock.Object;
        
        // when
        var results = (await handler.Handle(command, default)).ToArray();
        
        // then
        
        Assert.NotNull(results);
        Assert.Equal(2, results.Length);

        Assert.Equal(3, results[0].Id);
        Assert.Equal("preset3", results[0].Preset);
        Assert.Equal("mdl3", results[0].Model);
        
        Assert.Equal(5, results[1].Id);
        Assert.Equal("test_preset", results[1].Preset);
        Assert.Equal("mdl", results[1].Model);
    }
}