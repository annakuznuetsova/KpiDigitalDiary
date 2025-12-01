namespace DigitalDiary.DTOs
{
    public class HomePageCreateDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = "My Digital Space";
        public string Theme { get; set; } = "default";
        public List<string>? Tools { get; set; } = new() { "Create Entry", "View Profile", "Settings" };
    }
}
