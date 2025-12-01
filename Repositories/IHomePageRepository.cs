using DigitalDiary.Models;

namespace DigitalDiary.Repositories
{
    public interface IHomePageRepository
    {
        Task<List<HomePage>> GetAllAsync();
        Task<HomePage?> GetByIdAsync(int id);
        Task<HomePage?> GetByUserIdAsync(int userId);
        Task AddAsync(HomePage page);
        Task UpdateAsync(HomePage page);
        Task DeleteAsync(int id);
    }
}
