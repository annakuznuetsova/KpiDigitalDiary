namespace DigitalDiary.DTOs
{
    public class EntryUpdateDto
    {
        public int? UserId { get; set; }        
        public string? Title { get; set; }      
        public string? Content { get; set; }
        public List<string>? Tags { get; set; }
    }
}
