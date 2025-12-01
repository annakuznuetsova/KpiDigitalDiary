using DigitalDiary.Data;
using DigitalDiary.Models;
using DigitalDiary.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DigitalDiary.Tests
{
    public class HomePageServiceTests : System.IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DigitalDiaryContext _context;
        private readonly HomePageService _service;

        public HomePageServiceTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<DigitalDiaryContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new DigitalDiaryContext(options);
            _context.Database.EnsureCreated();

            _service = new HomePageService(_context);
        }

        [Fact]
        public async Task Add_WithMissingDefaults_AppliesDefaults()
        {
            var user = new User { Username = "hp1", Email = "hp@a.com", PasswordHash = "hash" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var page = new HomePage { UserId = user.Id, Title = "", Theme = "", Tools = null! };
            await _service.AddAsync(page);

            var fetched = await _service.GetByUserIdAsync(user.Id);
            Assert.NotNull(fetched);
            Assert.Equal("My Homepage", fetched.Title);
            Assert.Equal("default", fetched.Theme);
            Assert.NotNull(fetched.Tools);
            Assert.NotEmpty(fetched.Tools);
        }

        [Fact]
        public async Task Add_NonExistingUser_ThrowsArgumentException()
        {
            var page = new HomePage { UserId = 999, Title = "t", Theme = "x" };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(page));
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
