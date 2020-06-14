namespace APDB_CW_3.Models
{
    public class StudentSecurityData
    {
        public StudentSecurityData(string passwordHash, string salt, string role, string refreshToken)
        {
            PasswordHash = passwordHash;
            Salt = salt;
            Role = role;
            RefreshToken = refreshToken;
        }

        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public string Role { get; set; }

        public string RefreshToken { get; set; }
    }
}