using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareMove.Domain.Service;

public class AuthenticateService : IAuthenticateService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthenticateService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public string Authenticate(InputAuthenticate inputAuthenticate)
    {
        UserDTO? userDTO = _userRepository.GetFilterByCPF(inputAuthenticate.CPF);
        if (userDTO == null)
            throw new KeyNotFoundException("Usuário não existente");

        if (!string.Equals(userDTO.Password, inputAuthenticate.Password, StringComparison.Ordinal))
            throw new Exception("Senha inválida");

        return GenerateToken(userDTO);
    }

    #region Private
    private string GenerateToken(UserDTO userDTO)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userDTO.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userDTO.Email),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Criando o token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        string stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
    #endregion
}
