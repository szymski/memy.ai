using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;
using Domain.Credits.Interfaces;
using Domain.Credits.Services;

namespace Domain.UnitTests.Credits.Services;

public class CreditServiceTests {
    class CreditRepoMock : ICreditRepository {
        public IList<CreditEvent> CreditEvents { get; set; } = new List<CreditEvent>();

        public async Task<IEnumerable<CreditEvent>> GetCreditEvents(User user)
        {
            return CreditEvents
                .OrderByDescending(c => c.Id)
                .ThenByDescending(c => c.CreatedAt)
                .ToArray();
        }

        public Task<CreditEvent> AddCreditEvent(
            User user,
            CreditEventType type,
            decimal amount)
        {
            var creditEvent = new CreditEvent()
            {
                Id = CreditEvents.Count + 1,
                Type = type,
                Amount = amount,
            };
            CreditEvents.Add(creditEvent);
            return Task.FromResult(creditEvent);
        }
    }

    [Fact]
    public async Task should_add_events_to_repo()
    {
        // given
        var repo = new CreditRepoMock();
        var service = new CreditService(repo);
        var user = new User();

        // when
        await service.AddCreditEvent(user, CreditEventType.Seed, 1);
        await service.AddCreditEvent(user, CreditEventType.Used, 2);

        // then
        var events = await service.GetEvents(user);
        Assert.Equal(2, events.Count());
        var ev1 = events.ToArray()[0];
        var ev2 = events.ToArray()[1];

        Assert.Equal(CreditEventType.Used, ev1.Type);
        Assert.Equal(2, ev1.Amount);

        Assert.Equal(CreditEventType.Seed, ev2.Type);
        Assert.Equal(1, ev2.Amount);
    }
    
    [Fact]
    public async Task should_throw_when_taking_more_credits_than_available()
    {
        // given
        var repo = new CreditRepoMock();
        var service = new CreditService(repo);
        var user = new User();

        // when
        await service.AddCreditEvent(user, CreditEventType.Seed, 3);

        // then
        await service.AddCreditEvent(user, CreditEventType.Used, -1);
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.AddCreditEvent(user, CreditEventType.Used, -3);
        });
    }

    [Fact]
    public async Task should_return_balance()
    {
        // given
        var repo = new CreditRepoMock();
        var service = new CreditService(repo);
        var user = new User();

        // when
        await service.AddCreditEvent(user, CreditEventType.Seed, 3);
        await service.AddCreditEvent(user, CreditEventType.Used, -1);

        // then
        var balance = await service.GetCreditBalance(user);
        Assert.Equal(2, balance);
    }
}