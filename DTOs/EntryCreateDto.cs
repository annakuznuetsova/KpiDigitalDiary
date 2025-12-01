namespace DigitalDiary.DTOs
{
    public class EntryCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string>? Tags { get; set; }
        public int UserId { get; set; }
    }
}
