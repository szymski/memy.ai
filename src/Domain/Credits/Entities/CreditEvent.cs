using Domain.Auth.Entities;
using Domain.Common;
using Domain.Credits.Enums;

namespace Domain.Credits.Entities; 

public class CreditEvent : BaseEntity {
    public virtual User User { get; set; }
    public CreditEventType Type { get; set; }
    public decimal Amount { get; set; }
}