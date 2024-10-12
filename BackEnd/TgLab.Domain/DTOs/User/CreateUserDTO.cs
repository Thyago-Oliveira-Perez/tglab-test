using System.ComponentModel.DataAnnotations;

namespace TgLab.Domain.DTOs.User
{
    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        public bool IsUnder18()
        {
            var today = DateTime.Today;
            var age = today.Year - this.BirthDate.Year;

            // Adjust if the day of birth has not yet occurred
            if (this.BirthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age < 18;
        }
    }
}
