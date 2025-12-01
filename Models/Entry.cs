using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDiary.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        public string Content { get; set; } = string.Empty;

        public List<string>? Tags { get; set; } = new();

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
