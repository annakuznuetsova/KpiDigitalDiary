using DigitalDiary.DTOs;
using DigitalDiary.Models;
using DigitalDiary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DigitalDiary.Services
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _repository;

        public EntryService(IEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Entry>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Entry?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Entry entry)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(entry.Title))
                errors.Add("Entry title cannot be empty.");
            else if (entry.Title.Length < 3)
                errors.Add("Entry title must be at least 3 characters long.");

            if (string.IsNullOrWhiteSpace(entry.Content))
                errors.Add("Entry content cannot be empty.");
            else if (entry.Content.Length < 10)
                errors.Add("Entry content must be at least 10 characters long.");

            if (entry.UserId <= 0)
                errors.Add("Invalid UserId for entry.");
            else
            {
                bool userExists = await _repository.UserExistsAsync(entry.UserId);
                if (!userExists)
                    errors.Add($"User with ID {entry.UserId} does not exist.");
            }

            if (errors.Count > 0)
                throw new ArgumentException(string.Join(" ", errors));

            await _repository.AddAsync(entry);
        }

        public async Task<bool> UpdateAsync(int id, EntryUpdateDto updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            if (updated.UserId.HasValue && updated.UserId.Value != existing.UserId)
            {
                var userExists = await _repository.UserExistsAsync(updated.UserId.Value);
                if (!userExists)
                    throw new ArgumentException($"User with id = {updated.UserId.Value} does not exist.");

                existing.UserId = updated.UserId.Value;
            }

            if (!string.IsNullOrWhiteSpace(updated.Title))
            {
                if (updated.Title.Length < 3)
                    throw new ArgumentException("Entry title must be at least 3 characters long.");
                existing.Title = updated.Title;
            }

            if (!string.IsNullOrWhiteSpace(updated.Content))
            {
                if (updated.Content.Length < 10)
                    throw new ArgumentException("Entry content must be at least 10 characters long.");
                existing.Content = updated.Content;
            }

            if (updated.Tags != null)
                existing.Tags = updated.Tags;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task DeleteAllAsync()
        {
            var all = await _repository.GetAllAsync();
            foreach (var entry in all)
            {
                await _repository.DeleteAsync(entry.Id);
            }
        }
    }
}
