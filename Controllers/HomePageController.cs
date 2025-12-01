using DigitalDiary.DTOs;
using DigitalDiary.Models;
using DigitalDiary.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDiary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomePageController : ControllerBase
    {
        // HomePageController
        private readonly IHomePageService _pageService;
        private readonly IUserService _userService;

        public HomePageController(IHomePageService pageService, IUserService userService)
        {
            _pageService = pageService;
            _userService = userService;
        }

        // GET: api/homepage
        [HttpGet]
        public async Task<ActionResult<List<HomePageCreateDto>>> GetAll()
        {
            var pages = await _pageService.GetAllAsync();
            var dtoList = pages.Select(p => new HomePageCreateDto
            {
                UserId = p.UserId,
                Title = p.Title,
                Theme = p.Theme,
                Tools = p.Tools
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/homepage/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HomePageCreateDto>> GetById(int id)
        {
            var page = await _pageService.GetByIdAsync(id);
            if (page == null)
                return NotFound($"Home page with id = {id} not found.");

            var dto = new HomePageCreateDto
            {
                UserId = page.UserId,
                Title = page.Title,
                Theme = page.Theme,
                Tools = page.Tools
            };

            return Ok(dto);
        }

        // POST: api/homepage
        [HttpPost]
        public async Task<ActionResult<HomePageDto>> Add([FromBody] HomePageCreateDto newPageDto)
        {
            if (newPageDto == null)
                return BadRequest("Home page data is missing.");

            var user = await _userService.GetByIdAsync(newPageDto.UserId);
            if (user == null)
                return BadRequest($"User with ID {newPageDto.UserId} does not exist.");

            var existingPage = await _pageService.GetByUserIdAsync(newPageDto.UserId);
            if (existingPage != null)
                return Conflict($"User with ID {newPageDto.UserId} already has a home page.");

            var homePage = new HomePage
            {
                UserId = newPageDto.UserId,
                Title = newPageDto.Title,
                Theme = newPageDto.Theme,
                Tools = newPageDto.Tools ?? new List<string>()
            };

            await _pageService.AddAsync(homePage);

            var dto = new HomePageDto
            {
                Id = homePage.Id, 
                UserId = homePage.UserId,
                Title = homePage.Title,
                Theme = homePage.Theme,
                Tools = homePage.Tools
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/homepage/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] HomePageUpdateDto updatedDto)
        {
            if (updatedDto == null)
                return BadRequest("Home page data is missing.");

            var existing = await _pageService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Home page with id = {id} not found.");

            if (string.IsNullOrWhiteSpace(updatedDto.Title))
                existing.Title = "My Homepage";
            else
                existing.Title = updatedDto.Title;

            if (string.IsNullOrWhiteSpace(updatedDto.Theme))
                existing.Theme = "default";
            else
                existing.Theme = updatedDto.Theme;

            if (updatedDto.Tools != null && updatedDto.Tools.Count > 0)
                existing.Tools = updatedDto.Tools;

            await _pageService.UpdateAsync(id, existing);

            return Ok(new HomePageDto
            {
                Id = existing.Id,
                UserId = existing.UserId,
                Title = existing.Title,
                Theme = existing.Theme,
                Tools = existing.Tools
            });
        }

        // DELETE: api/homepage/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _pageService.DeleteAsync(id);
            if (!success)
                return NotFound($"Home page with id = {id} not found.");
            return Ok($"Home page with id = {id} deleted.");
        }

        // DELETE: api/homepage
        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            await _pageService.DeleteAllAsync();
            return Ok("All home pages deleted.");
        }
    }
}
