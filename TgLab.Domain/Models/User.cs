using System.ComponentModel.DataAnnotations;

namespace TgLab.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        public virtual IEnumerable<Wallet> Wallets { get; set; }
    }
}
