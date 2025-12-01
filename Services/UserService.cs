using DigitalDiary.Data;
using DigitalDiary.DTOs;
using DigitalDiary.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DigitalDiary.Services
{
    public class UserService : IUserService
    {
        private readonly DigitalDiaryContext _context;

        public UserService(DigitalDiaryContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> AddAsync(UserCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Please enter a valid username — it cannot be empty.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email cannot be empty.");

            if (!dto.Email.Contains('@'))
                throw new ArgumentException("Invalid email format. Please provide a valid one (example@domain.com).");

            bool exists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email || u.Username == dto.Username);
            if (exists)
                throw new InvalidOperationException("User with this email or username already exists.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password cannot be empty.");

            if (dto.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.");

            var hashed = HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Bio = dto.Bio,
                PasswordHash = hashed
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;

            var hashed = HashPassword(password);
            return user.PasswordHash == hashed ? user : null;
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            var errors = new List<string>();

            if (dto.Username != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Username))
                    errors.Add("Username cannot be empty.");
                else
                    user.Username = dto.Username;
            }

            if (dto.Email != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
                    errors.Add("Invalid email format. Please provide a valid one (example@domain.com).");
                else
                    user.Email = dto.Email;
            }

            if (dto.Bio != null)
                user.Bio = dto.Bio;

            if (dto.Password != null)
            {
                if (dto.Password.Length < 6)
                    errors.Add("Password must be at least 6 characters long.");
                else
                    user.PasswordHash = HashPassword(dto.Password);
            }

            if (errors.Count > 0)
                throw new ArgumentException(string.Join(" ", errors));

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAllAsync()
        {
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }
    }
}
