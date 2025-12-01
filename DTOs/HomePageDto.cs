namespace DigitalDiary.DTOs
{
    public class HomePageDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public List<string>? Tools { get; set; } = new();
    }
}
