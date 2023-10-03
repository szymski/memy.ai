using Mapster;

namespace WebApi.Controllers.Models;

public class AccountPatchRequestDto {
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    [AdaptMember("PasswordHash")]
    public string? Password { get; set; }
}