using Application.Common.Interfaces;
using Application.Stories.Queries;
using Application.UnitTests.Helpers;
using Domain.Stories.Entities;

namespace Application.UnitTests.Stories.Queries; 

public class GetStoryQueryTests {
    [Fact]
    public async Task Handle_Should_Return_Story_By_Id()
    {
        // given

        var stories = new List<Story>()
        {
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

        var command = new GetStoryQuery(5);
        var handler = new GetStoryQuery.GetStoryQueryHandler();
        handler.Context = contextMock.Object;
        
        // when
        var result = await handler.Handle(command, default);
        
        // then
        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("test_preset", result.Preset);
        Assert.Equal("mdl", result.Model);
    }
}