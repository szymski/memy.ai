using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.UnitTests.Helpers;

public static class DbHelper {
    public static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> entities)
        where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
        mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>((object[] param) => entities.SingleOrDefault(e => (e as BaseEntity).Id == (int)param[0]));
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>((object[] param) => ValueTask.FromResult(entities.SingleOrDefault(e => (e as BaseEntity).Id == (int)param[0])));
        }
        return mockSet;
    }
}