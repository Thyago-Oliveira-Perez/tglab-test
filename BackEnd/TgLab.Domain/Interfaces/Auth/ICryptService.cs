namespace TgLab.Domain.Interfaces.Auth
{
    public interface ICryptService
    {
        public string HashPassword(string password);

        public bool InvalidPassword(string password, string savedPassword);
    }
}
