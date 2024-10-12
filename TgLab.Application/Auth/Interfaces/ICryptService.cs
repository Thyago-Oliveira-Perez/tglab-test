namespace TgLab.Application.Auth.Interfaces
{
    public interface ICryptService
    {
        public string HashPassword(string password);

        public bool InvalidPassword(string password, string savedPassword);
    }
}
