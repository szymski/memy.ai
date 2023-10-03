using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;
using Domain.Credits.Interfaces;

namespace Domain.Credits.Services; 

public class CreditService(ICreditRepository repo) {
    public async Task<decimal> GetCreditBalance(User user)
    {
        var credits = await repo.GetCreditEvents(user);
        return credits.Sum(c => c.Amount);
    }

    public async Task<IEnumerable<CreditEvent>> GetEvents(User user)
    {
        return await repo.GetCreditEvents(user);
    }
    
    public async Task<CreditEvent> AddCreditEvent(
        User user,
        CreditEventType type,
        decimal amount)
    {
        if (amount <= 0)
        {
            var balance = await GetCreditBalance(user);
            var newBalance = balance + amount;
            if (newBalance < 0)
                throw new("Not enough credits");
        }
        
        return await repo.AddCreditEvent(user, type, amount);
    }
}