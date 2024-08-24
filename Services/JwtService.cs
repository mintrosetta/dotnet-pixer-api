using Microsoft.IdentityModel.Tokens;
using PixerAPI.Models;
using PixerAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PixerAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly string key;
        private readonly string issuer;
        private readonly string audience;

        public JwtService(IConfiguration configuration)
        {
            this.key = configuration.GetSection("Jwt:Key").Get<string>()!;
            this.issuer = configuration.GetSection("Jwt:Issuer").Get<string>()!;
            this.audience = configuration.GetSection("Jwt:Audience").Get<string>()!;
        }

        public string GenerateToken(User user)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] keyByte = Encoding.UTF8.GetBytes(this.key);
                SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    }),
                    Issuer = this.issuer,
                    Audience = this.audience,
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyByte), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken securityToken = tokenHandler.CreateToken(securityTokenDescriptor);

                return tokenHandler.WriteToken(securityToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
