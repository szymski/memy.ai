using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.Models; 

public class RegisterRequestDto {
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}