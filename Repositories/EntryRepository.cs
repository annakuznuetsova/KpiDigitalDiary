using DigitalDiary.Data;
using DigitalDiary.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalDiary.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly DigitalDiaryContext _context;

        public EntryRepository(DigitalDiaryContext context)
        {
            _context = context;
        }

        public async Task<List<Entry>> GetAllAsync()
        {
            return await _context.Entries.ToListAsync();
        }

        public async Task<Entry?> GetByIdAsync(int id)
        {
            return await _context.Entries.FindAsync(id);
        }

        public async Task AddAsync(Entry entry)
        {
            await _context.Entries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Entry entry)
        {
            _context.Entries.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _context.Entries.FindAsync(id);
            if (entry != null)
            {
                _context.Entries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
