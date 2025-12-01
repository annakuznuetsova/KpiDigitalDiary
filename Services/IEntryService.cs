using DigitalDiary.DTOs;
using DigitalDiary.Models;

namespace DigitalDiary.Services
{
    public interface IEntryService
    {
        Task<List<Entry>> GetAllAsync();
        Task<Entry?> GetByIdAsync(int id);
        Task AddAsync(Entry entry);
        Task<bool> UpdateAsync(int id, EntryUpdateDto updated);
        Task<bool> DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
