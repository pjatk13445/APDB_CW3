using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APDB_CW_3.Services
{
    public class SecurityService
    {
        public JwtSecurityToken GenerateToken(string IndexNumber, string Role)
        {
            ICollection<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, IndexNumber));
            claims.Add(new Claim(ClaimTypes.Role, Role));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));

            var token = new JwtSecurityToken
            (
                issuer: "MarekLewandowski",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private IConfiguration Configuration { get; set; }

        public SecurityService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}