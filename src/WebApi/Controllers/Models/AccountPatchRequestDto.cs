namespace WebApi.Controllers.Models;

public class AccountPatchRequestDto {
    public string? DisplayName { get; set; }
    public string Password { get; set; }
}