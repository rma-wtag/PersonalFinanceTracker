using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using PersonalFinanceTracker.Dtos.UserDtos;
using PersonalFinanceTracker.Entities;
using PersonalFinanceTracker.Mappers;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetStudentById([FromRoute] int id) {
            var student = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (student == null) {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto) {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = userDto.ToUserFromCreateDto();
            await _dbContext.Users.AddAsync(userModel);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetStudentById), new { id = userModel.UserId }, userModel);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateUserDto userDto) {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();

            user.UserName = userDto.UserName;
            user.Balance = userDto.Balance;

            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }
        [HttpDelete("{id:int}")]
        public async Task<User?> Delete([FromRoute] int id) {
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null)
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else return null;

            return user;
        }
    }
}