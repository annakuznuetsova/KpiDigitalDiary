using System.ComponentModel.DataAnnotations;

namespace DigitalDiary.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string PasswordHash { get; set; } = string.Empty;

        public List<Entry>? Entries { get; set; }

        public HomePage? HomePage { get; set; }
    }
}
