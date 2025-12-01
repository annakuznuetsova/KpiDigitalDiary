using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDiary.Models
{
    public class HomePage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = "My Digital Space";

        [Required]
        [MaxLength(50)]
        public string Theme { get; set; } = "default";

        public List<string> Tools { get; set; } = new()
        {
            "Create Entry",
            "View Profile",
            "Settings"
        };

        public User? User { get; set; }
    }
}
