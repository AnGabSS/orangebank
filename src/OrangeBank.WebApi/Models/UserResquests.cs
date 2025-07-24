namespace OrangeBank.WebApi.Models;

public class RegisterUserRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Cpf { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required string Password { get; set; } 
}

public class ChangePasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

public class PasswordResetRequest
{
    public required string Email { get; set; }
}

