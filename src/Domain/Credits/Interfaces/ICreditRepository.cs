using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;

namespace Domain.Credits.Interfaces; 

public interface ICreditRepository {
    Task<IEnumerable<CreditEvent>> GetCreditEvents(User user);
    Task<CreditEvent> AddCreditEvent(User user, CreditEventType type, decimal amount);
}