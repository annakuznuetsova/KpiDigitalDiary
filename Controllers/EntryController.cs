using DigitalDiary.DTOs;
using DigitalDiary.Models;
using DigitalDiary.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDiary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntryController : ControllerBase
    {
        // EntryController (конструктор)
        private readonly IEntryService _entryService;
        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        // GET: api/entry
        [HttpGet]
        public async Task<ActionResult<List<EntryCreateDto>>> GetAll()
        {
            var entries = await _entryService.GetAllAsync();
            var result = entries.Select(e => new EntryCreateDto
            {
                Title = e.Title,
                Content = e.Content,
                Tags = e.Tags,
                UserId = e.UserId
            }).ToList();

            return Ok(result);
        }

        // GET: api/entry/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EntryReadDto>> GetById(int id)
        {
            var entry = await _entryService.GetByIdAsync(id);
            if (entry == null)
                return NotFound($"Entry with id = {id} not found.");

            var dto = new EntryReadDto
            {
                Id = entry.Id,
                Title = entry.Title,
                Content = entry.Content,
                Tags = entry.Tags,
                UserId = entry.UserId
            };

            return Ok(dto);
        }

        // POST: api/entry
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] EntryCreateDto newEntryDto)
        {
            if (newEntryDto == null)
                return BadRequest("Entry data is missing.");

            var newEntry = new Entry
            {
                Title = newEntryDto.Title,
                Content = newEntryDto.Content,
                Tags = newEntryDto.Tags ?? new List<string>(),
                UserId = newEntryDto.UserId
            };

            try
            {
                await _entryService.AddAsync(newEntry);
                return CreatedAtAction(nameof(GetById), new { id = newEntry.Id }, newEntry);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // PUT: api/entry/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EntryUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Updated entry data is missing.");

            try
            {
                var success = await _entryService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound($"Entry with id = {id} not found.");

                return Ok($"Entry with id = {id} updated.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // DELETE: api/entry/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _entryService.DeleteAsync(id);
            if (!success)
                return NotFound($"Entry with id = {id} not found.");
            return Ok($"Entry with id = {id} deleted.");
        }

        // DELETE: api/entry
        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            await _entryService.DeleteAllAsync();
            return Ok("All entries deleted.");
        }
    }
}
