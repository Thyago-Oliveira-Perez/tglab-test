namespace TgLab.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual IEnumerable<Wallet> Wallets { get; set; }
    }
}
