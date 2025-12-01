using DigitalDiary.Data;
using DigitalDiary.Models;
using DigitalDiary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DigitalDiary.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly DigitalDiaryContext _context;

        public HomePageService(DigitalDiaryContext context)
        {
            _context = context;
        }

        public async Task<List<HomePage>> GetAllAsync()
        {
            return await _context.HomePages.ToListAsync();
        }

        public async Task<HomePage?> GetByIdAsync(int id)
        {
            return await _context.HomePages.FindAsync(id);
        }

        public async Task AddAsync(HomePage page)
        {
            var errors = new List<string>();

            var userExists = await _context.Users.AnyAsync(u => u.Id == page.UserId);
            if (!userExists)
                errors.Add($"User with ID {page.UserId} does not exist.");

            if (string.IsNullOrWhiteSpace(page.Title))
                page.Title = "My Homepage";

            if (string.IsNullOrWhiteSpace(page.Theme))
                page.Theme = "default";

            if (page.Tools == null || page.Tools.Count == 0)
                page.Tools = new List<string> { "Create Entry", "View Profile", "Settings" };

            if (errors.Count > 0)
                throw new ArgumentException(string.Join(" ", errors));

            await _context.HomePages.AddAsync(page);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, HomePage updated)
        {
            var existing = await _context.HomePages.FindAsync(id);
            if (existing == null)
                return false;

            if (!string.IsNullOrWhiteSpace(updated.Title))
                existing.Title = updated.Title;
            if (!string.IsNullOrWhiteSpace(updated.Theme))
                existing.Theme = updated.Theme;
            if (updated.Tools != null && updated.Tools.Count > 0)
                existing.Tools = updated.Tools;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<HomePage?> GetByUserIdAsync(int userId)
        {
            return await _context.HomePages.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var page = await _context.HomePages.FindAsync(id);
            if (page == null)
                return false;

            _context.HomePages.Remove(page);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAllAsync()
        {
            _context.HomePages.RemoveRange(_context.HomePages);
            await _context.SaveChangesAsync();
        }
    }
}
