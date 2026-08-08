using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Store.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);

        }

        public async Task<string> GenerateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var authClaim = new List<Claim>
            {
               new Claim(ClaimTypes.Name,user.DisplayName),
               new Claim(ClaimTypes.Email,user.Email),
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var token=new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: authClaim,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"]))
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
