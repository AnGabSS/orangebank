using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface ICheckingAccountService
    {
        Task<User> RegisterCheckingAccountAscync(string name, string email, string cpf, string phoneNumber, DateOnly birthDate, string password);
        Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task RequestPasswordResetAsync(string email);
        Task<string?> AuthenticateUserAsync(string email, string password);
    }
}
