using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Credits.Enums;

namespace Domain.Credits.Interfaces; 

public interface ICreditRepository {
    Task<IEnumerable<CreditEvent>> GetCreditEvents(int userId);
    Task<CreditEvent> AddCreditEvent(int user, CreditEventType type, decimal amount);
}