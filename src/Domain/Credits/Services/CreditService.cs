using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;
using Domain.Credits.Interfaces;

namespace Domain.Credits.Services; 

public class CreditService(ICreditRepository repo) {
    public async Task<decimal> GetCreditBalance(int userId)
    {
        var credits = await repo.GetCreditEvents(userId);
        return credits.Sum(c => c.Amount);
    }

    public async Task<IEnumerable<CreditEvent>> GetEvents(int userId)
    {
        return await repo.GetCreditEvents(userId);
    }
    
    public async Task<CreditEvent> AddCreditEvent(
        int userId,
        CreditEventType type,
        decimal amount)
    {
        if (amount <= 0)
        {
            var balance = await GetCreditBalance(userId);
            var newBalance = balance + amount;
            if (newBalance < 0)
                throw new("Not enough credits");
        }
        
        return await repo.AddCreditEvent(userId, type, amount);
    }
}