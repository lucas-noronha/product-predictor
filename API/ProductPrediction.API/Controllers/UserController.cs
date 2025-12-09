using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductPrediction.API.Dtos;
using ProductPrediction.API.Entities;
using ProductPrediction.API.Infrastructure;

namespace ProductPrediction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public AppDbContext Db { get; }
        public UsersController(AppDbContext db)
        {
            Db = db;
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] UserCreateDto userInfo)
        {
            var user = Db.User.FirstOrDefault(u => u.Email == userInfo.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { user.Id, user.Name, user.Email });
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserCreateDto userInfo)
        {
            Db.User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = userInfo.Name,
                Email = userInfo.Email
            });
            Db.SaveChanges();
            return Ok("User registered successfully.");
        }
    }
}
