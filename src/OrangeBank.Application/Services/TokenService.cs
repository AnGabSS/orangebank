using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrangeBank.Core.Domain.Security;
using DotNetEnv;

public class TokenService: ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username)
    {
        Env.Load("../../.env");
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyString = Env.GetString("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET is not set in the environment variables.");
        var key = Encoding.UTF8.GetBytes(keyString);
        var expiresIn = Env.GetString("JWT_EXPIRE_HOURS") ?? "1"; // Default to 1 hour if not set

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

        //foreach (var role in roles)
        //{
        //    claims.Add(new Claim(ClaimTypes.Role, role));
        //}

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(expiresIn)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}