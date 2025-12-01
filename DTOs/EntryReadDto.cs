namespace DigitalDiary.DTOs
{
    public class EntryReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string>? Tags { get; set; }
        public int UserId { get; set; }
    }
}
