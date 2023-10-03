namespace WebApi.Dto; 

public class MyAccountDto : IBaseEntityDto {
    public int Id { get; set; }
    public string Email { get; set; }
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
}