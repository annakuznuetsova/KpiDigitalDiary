// IUserService.cs
using DigitalDiary.DTOs;
using DigitalDiary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalDiary.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(UserCreateDto dto);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<bool> UpdateAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
