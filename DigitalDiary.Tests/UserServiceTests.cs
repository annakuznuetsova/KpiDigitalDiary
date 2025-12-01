using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using DigitalDiary.Data;
using DigitalDiary.Services;
using DigitalDiary.DTOs;
using DigitalDiary.Models;
using System;

namespace DigitalDiary.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DigitalDiaryContext _context;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<DigitalDiaryContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new DigitalDiaryContext(options);
            _context.Database.EnsureCreated();

            _service = new UserService(_context);
        }

        [Fact]
        public async Task Add_ValidUser_ReturnsUserWithHashedPassword()
        {
            var dto = new UserCreateDto { Username = "u1", Email = "a@b.com", Password = "secret123", Bio = "bio" };
            var user = await _service.AddAsync(dto);

            Assert.NotNull(user);
            Assert.Equal(dto.Username, user.Username);
            Assert.Equal(dto.Email, user.Email);
            Assert.NotEqual(dto.Password, user.PasswordHash); // should be hashed
        }

        [Fact]
        public async Task Add_DuplicateUser_ThrowsInvalidOperationException()
        {
            var dto = new UserCreateDto { Username = "u2", Email = "b@b.com", Password = "secret" };
            await _service.AddAsync(dto);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsUser()
        {
            var dto = new UserCreateDto { Username = "u3", Email = "c@c.com", Password = "pass123" };
            var created = await _service.AddAsync(dto);

            var auth = await _service.AuthenticateAsync(dto.Username, dto.Password);
            Assert.NotNull(auth);
            Assert.Equal(created.Id, auth.Id);
        }

        [Fact]
        public async Task Authenticate_InvalidPassword_ReturnsNull()
        {
            var dto = new UserCreateDto { Username = "u4", Email = "d@d.com", Password = "pass123" };
            await _service.AddAsync(dto);

            var auth = await _service.AuthenticateAsync(dto.Username, "wrong");
            Assert.Null(auth);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
