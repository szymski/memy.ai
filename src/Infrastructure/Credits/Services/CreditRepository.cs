using Application.Common.Interfaces;
using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;
using Domain.Credits.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Credits.Services;

public class CreditRepository(
    IAppDbContext context) : ICreditRepository {
    public async Task<IEnumerable<CreditEvent>> GetCreditEvents(int userId)
    {
        return await context.CreditEvents
            .Where(c => c.User.Id == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToArrayAsync();
    }

    public async Task<CreditEvent> AddCreditEvent(
        int userId,
        CreditEventType type,
        decimal amount)
    {
        var user = await context.Users.FindAsync(userId);
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