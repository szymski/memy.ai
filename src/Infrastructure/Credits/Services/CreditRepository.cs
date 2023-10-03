using Application.Common.Interfaces;
using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;
using Domain.Credits.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Credits.Services;

public class CreditRepository(
    IAppDbContext context) : ICreditRepository {
    public async Task<IEnumerable<CreditEvent>> GetCreditEvents(User user)
    {
        return await context.CreditEvents
            .Where(c => c.User == user)
            .OrderByDescending(c => c.CreatedAt)
            .ToArrayAsync();
    }

    public async Task<CreditEvent> AddCreditEvent(
        User user,
        CreditEventType type,
        decimal amount)
    {
        var creditEvent = new CreditEvent()
        {
            Type = type,
            Amount = amount,
        };
        user.CreditEvents.Add(creditEvent);
        await context.SaveChangesAsync();
        return creditEvent;
    }
}