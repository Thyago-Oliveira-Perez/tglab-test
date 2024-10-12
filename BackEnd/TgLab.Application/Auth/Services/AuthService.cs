using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TgLab.Application.Auth.DTOs;
using TgLab.Application.Auth.Interfaces;
using TgLab.Application.User.DTOs;
using TgLab.Application.Wallet.DTOs;
using TgLab.Infrastructure.Context;

namespace TgLab.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly TgLabContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICryptService _cryptService;

        public AuthService(TgLabContext context, IConfiguration configuration, ICryptService cryptService)
        {
            _context = context;
            _configuration = configuration;
            _cryptService = cryptService;
        }

        public async Task<LoggedUserDTO> Login(LoginDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Wallets)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || _cryptService.InvalidPassword(dto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var userDto = new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Wallets = user.Wallets.Select(w => new WalletDTO
                {
                    Id = w.Id,
                    Balance = w.Balance,
                    Currency = w.Currency
                }).ToList()
            };

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);


            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new LoggedUserDTO(userDto, token);
        }
    }
}
