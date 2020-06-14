using System;
using APDB_CW_3.Models;
using APDB_CW_3.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace APDB_CW_3.Controllers
{
    [ApiController]
    [Route("api/security")]
    public class SecurityController : ControllerBase
    {
        private IStudentsDbService _db;
        private SecurityService _security;

        public SecurityController(IStudentsDbService db, SecurityService security)
        {
            _db = db;
            _security = security;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginPayload payload)
        {
            if (!_db.StudentExists(payload.IndexNumber))
            {
                return Unauthorized("User not found");
            }

            var SecurityData = _db.GetStudentSecurityData(payload.IndexNumber);

            Console.WriteLine(SecurityData.PasswordHash);
            if (String.IsNullOrEmpty(SecurityData.PasswordHash))
            {
                var Salt = HashingService.GenerateSalt();
                _db.UpdatePassword(
                    payload.IndexNumber,
                    Salt,
                    HashingService.Hash(payload.PlainPassword, Salt)
                );
            }
            else if (!HashingService.Check(
                payload.PlainPassword,
                SecurityData.Salt,
                SecurityData.PasswordHash
            ))
            {
                return Unauthorized("Wrong password");
            }

            var RefreshToken = Guid.NewGuid();
            _db.UpdateRefreshToken(payload.IndexNumber, RefreshToken.ToString());

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(_security.GenerateToken(
                    payload.IndexNumber,
                    SecurityData.Role
                )),
                RefreshToken = RefreshToken
            });
        }

        [HttpPost("refreshToken")]
        public IActionResult RefreshToken(RefreshTokenPayload payload)
        {
            if (!_db.StudentExists(payload.IndexNumber))
            {
                return Unauthorized("Invalid user or refresh token");
            }

            var SecurityData = _db.GetStudentSecurityData(payload.IndexNumber);
            if (payload.RefreshToken != SecurityData.RefreshToken)
            {
                return Unauthorized("Invalid user or refresh token");
            }

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(_security.GenerateToken(
                    payload.IndexNumber,
                    SecurityData.Role
                ))
            });
        }
    }
}