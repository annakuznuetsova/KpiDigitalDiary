using System;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DigitalDiary.Data;
using DigitalDiary.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DigitalDiary.Tests
{
    public class ConversionIntegrationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DigitalDiaryContext _context;

        public ConversionIntegrationTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DigitalDiaryContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new DigitalDiaryContext(options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task HomePage_Tools_And_Entry_Tags_Persist()
        {
            var user = new User { Username = "c1", Email = "x@x.com", PasswordHash = "h" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var page = new HomePage
            {
                UserId = user.Id,
                Title = "T",
                Theme = "dark",
                Tools = new List<string> { "A", "B" }
            };
            _context.HomePages.Add(page);

            var entry = new Entry
            {
                Title = "E",
                Content = "Content long enough here",
                UserId = user.Id,
                Tags = new List<string> { "t1", "t2" }
            };
            _context.Entries.Add(entry);

            await _context.SaveChangesAsync();

            var p = await _context.HomePages.FirstOrDefaultAsync(h => h.UserId == user.Id);
            var e = await _context.Entries.FirstOrDefaultAsync(x => x.Id == entry.Id);

            Assert.NotNull(p);
            Assert.NotNull(e);
            Assert.Contains("A", p.Tools);
            Assert.Contains("t1", e.Tags!);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
