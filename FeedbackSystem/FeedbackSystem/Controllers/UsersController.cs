using FeedbackSystem.Data;
using FeedbackSystem.Models;
using FeedbackSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //00016599
        private readonly FeedBackSysDbContext dbContext;

        public UsersController(FeedBackSysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await dbContext.Users
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    UserEmail = u.UserEmail
                })
                .ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await dbContext.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    UserEmail = u.UserEmail
                })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            return user;
        }
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDto)
        {
            var user = new Users
            {
                UserName = createUserDto.UserName,
                UserEmail = createUserDto.UserEmail
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var userDto = new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, userDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); 
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, CreateUserDTO updateUserDto)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); 
            }

            user.UserName = updateUserDto.UserName;
            user.UserEmail = updateUserDto.UserEmail;

            dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(); 
                }
                else
                {
                    throw; 
                }
            }

            return NoContent(); 
        }

        private bool UserExists(int id)
        {
            return dbContext.Users.Any(u => u.UserId == id);
        }


    }
}
