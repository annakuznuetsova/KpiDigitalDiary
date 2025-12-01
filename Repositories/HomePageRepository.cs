using DigitalDiary.Data;
using DigitalDiary.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalDiary.Repositories
{
    public class HomePageRepository : IHomePageRepository
    {
        private readonly DigitalDiaryContext _context;

        public HomePageRepository(DigitalDiaryContext context)
        {
            _context = context;
        }

        public async Task<List<HomePage>> GetAllAsync() =>
            await _context.HomePages.ToListAsync();

        public async Task<HomePage?> GetByIdAsync(int id) =>
            await _context.HomePages.FindAsync(id);

        public async Task<HomePage?> GetByUserIdAsync(int userId) =>
            await _context.HomePages.FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task AddAsync(HomePage page)
        {
            await _context.HomePages.AddAsync(page);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HomePage page)
        {
            _context.HomePages.Update(page);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var page = await _context.HomePages.FindAsync(id);
            if (page != null)
            {
                _context.HomePages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }
    }
}
