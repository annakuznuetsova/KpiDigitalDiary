// IHomePageService.cs
using DigitalDiary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalDiary.Services
{
    public interface IHomePageService
    {
        Task<List<HomePage>> GetAllAsync();
        Task<HomePage?> GetByIdAsync(int id);
        Task AddAsync(HomePage page);
        Task<bool> UpdateAsync(int id, HomePage updated);
        Task<HomePage?> GetByUserIdAsync(int userId);
        Task<bool> DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
