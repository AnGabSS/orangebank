using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(string name, string email, string cpf, string phoneNumber, DateOnly birthDate, string password);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task RequestPasswordResetAsync(string email);
    Task<string?> AuthenticateUserAsync(string email, string password);

}