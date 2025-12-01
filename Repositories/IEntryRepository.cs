using DigitalDiary.Models;

namespace DigitalDiary.Repositories
{
    public interface IEntryRepository
    {
        Task<List<Entry>> GetAllAsync();
        Task<Entry?> GetByIdAsync(int id);
        Task AddAsync(Entry entry);
        Task UpdateAsync(Entry entry);
        Task DeleteAsync(int id);
        Task<bool> UserExistsAsync(int userId);
    }
}
