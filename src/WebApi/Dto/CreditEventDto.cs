using Domain.Credits.Enums;

namespace WebApi.Dto; 

public class CreditEventDto : IBaseEntityDto {
    public int Id { get; set; }
    public CreditEventType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}