using DigitalDiary.DTOs;
using DigitalDiary.Models;
using DigitalDiary.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDiary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // UserController
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<List<UserReadDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();

            var dtoList = users.Select(u => new UserReadDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Bio = u.Bio
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with id = {id} not found.");

            var dto = new UserReadDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Bio = user.Bio
            };

            return Ok(dto);
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Add([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest("User data is missing.");

            try
            {
                var created = await _userService.AddAsync(dto);

                var readDto = new UserReadDto
                {
                    Id = created.Id,
                    Username = created.Username,
                    Email = created.Email,
                    Bio = created.Bio
                };

                return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            try
            {
                var success = await _userService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound($"User with id = {id} not found.");

                return Ok($"User with id = {id} updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success)
                return NotFound($"User with id = {id} not found.");

            return Ok($"User with id = {id} deleted.");
        }

        // DELETE: api/user
        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            await _userService.DeleteAllAsync();
            return Ok("All users deleted.");
        }
    }
}
