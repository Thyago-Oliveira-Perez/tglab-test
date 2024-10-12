using TgLab.Application.Auth.Interfaces;
using Crypt = BCrypt.Net.BCrypt;

namespace TgLab.Application.Auth.Services
{
    public class CryptService : ICryptService
    {
        public CryptService() { }

        public string HashPassword(string password)
        {
            return Crypt.HashPassword(password);
        }

        public bool InvalidPassword(string password, string savedPassword)
        {
            return !Crypt.Verify(password, savedPassword);
        }
    }
}
