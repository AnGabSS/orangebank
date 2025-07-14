using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.Core.Domain.Security;

namespace OrangeBank.Application.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<User> RegisterUserAsync(
        string name,
        string email,
        string cpf,
        string phoneNumber,
        DateOnly birthDate,
        string password)
    {
        if (await _userRepository.EmailExistsAsync(email))
            throw new ArgumentException("Email already registered");

        if (await _userRepository.EmailExistsAsync(cpf))
            throw new ArgumentException("CPF already registered");

        var user = new User(name, email, cpf, password, birthDate, phoneNumber);
        user.Password = _passwordHasher.HashPassword(password);

        Console.WriteLine("Registrando usuario");
        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new ArgumentException("User not found");

        if (!_passwordHasher.VerifyHashedPassword(currentPassword, user.Password))
            throw new ArgumentException("Current password is incorrect");

        user.Password = _passwordHasher.HashPassword(newPassword);
        await _userRepository.UpdateAsync(user);
    }

    public async Task RequestPasswordResetAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return; // Segurança: não revelar que email não existe  

        // Lógica para gerar e enviar token de reset  
    }

    public async Task<string> AuthenticateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new ArgumentException("User not found");

        if (!_passwordHasher.VerifyHashedPassword(user.Password, password))
            throw new ArgumentException("Invalid password");

        var token = _tokenService.GenerateToken(user.Name);
        return token;
    }

}