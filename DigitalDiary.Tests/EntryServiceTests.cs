using DigitalDiary.DTOs;
using DigitalDiary.Models;
using DigitalDiary.Repositories;
using DigitalDiary.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DigitalDiary.Tests
{
    public class EntryServiceTests
    {
        private readonly Mock<IEntryRepository> _repoMock;
        private readonly IEntryService _service;

        public EntryServiceTests()
        {
            _repoMock = new Mock<IEntryRepository>();
            _service = new EntryService(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ValidEntry_CallsAddAsync()
        {
            var entry = new Entry { Title = "Hello", Content = "This is a valid content", UserId = 1, Tags = new List<string> { "a" } };
            _repoMock.Setup(r => r.UserExistsAsync(entry.UserId)).ReturnsAsync(true);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Entry>())).Returns(Task.CompletedTask).Verifiable();

            await _service.AddAsync(entry);

            _repoMock.Verify(r => r.AddAsync(It.Is<Entry>(e => e.Title == entry.Title && e.UserId == entry.UserId)), Times.Once);
        }

        [Theory]
        [InlineData("", "Valid content longer than ten", 1)]
        [InlineData("Hi", "Valid content longer than ten", 1)]
        [InlineData("Valid title", "", 1)]
        [InlineData("Valid title", "short", 1)]
        public async Task Create_InvalidData_ThrowsArgumentException(string title, string content, int userId)
        {
            var entry = new Entry { Title = title, Content = content, UserId = userId };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(entry));
        }

        [Fact]
        public async Task Create_NonExistingUser_ThrowsArgumentException()
        {
            var entry = new Entry { Title = "Valid title", Content = "Valid content with enough length", UserId = 999 };
            _repoMock.Setup(r => r.UserExistsAsync(entry.UserId)).ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(entry));
            Assert.Contains("does not exist", ex.Message);
        }

        [Fact]
        public async Task Update_ExistingEntry_UpdatesAndReturnsTrue()
        {
            var existing = new Entry { Id = 1, Title = "Old", Content = "Old content long enough", UserId = 1, Tags = new List<string> { "x" } };
            var updateDto = new EntryUpdateDto { Title = "New", Content = "New content longer than ten", Tags = new List<string> { "y" } };

            _repoMock.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Entry>())).Returns(Task.CompletedTask).Verifiable();

            var result = await _service.UpdateAsync(existing.Id, updateDto);

            Assert.True(result);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Entry>(e => e.Title == "New" && e.Content == updateDto.Content && e.Tags.Contains("y"))), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingEntry_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Entry?)null);

            var result = await _service.UpdateAsync(42, new EntryUpdateDto { Title = "x" });

            Assert.False(result);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ExistingEntry_ReturnsTrueAndCallsDelete()
        {
            var entry = new Entry { Id = 1, Title = "T", Content = "Content", UserId = 1 };
            _repoMock.Setup(r => r.GetByIdAsync(entry.Id)).ReturnsAsync(entry);
            _repoMock.Setup(r => r.DeleteAsync(entry.Id)).Returns(Task.CompletedTask).Verifiable();

            var result = await _service.DeleteAsync(entry.Id);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entry.Id), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingEntry_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Entry?)null);

            var result = await _service.DeleteAsync(123);

            Assert.False(result);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
