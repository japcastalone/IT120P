using IT120P.Data;
using IT120P.Models;
using IT120P.Services;
using Microsoft.AspNetCore.Mvc;

namespace IT120P.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly DeviceDbContext _dbContext;

        public UserController(AuthService authService, DeviceDbContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (_authService.Authenticate(loginRequest.Username, loginRequest.Password))
            {
                return Ok("Login Successful");
            }
            else
            {
                return Unauthorized("User not found");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_authService.Register(user))
            {
                return Ok("Registration Successful");
            }
            else
            {
                return BadRequest("Username already exists");
            }
        }

        [HttpGet("listall")]
        public IActionResult GetAllUsers()
        {
            var users = _dbContext.Users.ToList();

            if (users.Count == 0)
            {
                return Ok("No users found.");
            }
            else
            {
                return Ok(users);
            }
        }

        [HttpPut("edit/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            string message = $"User {user.Username} updated successfully. Changes made:\n";

            // Compare old and new values and update the fields that have changed in the request body
            if (!string.IsNullOrEmpty(updatedUser.Username) && user.Username != updatedUser.Username)
            {
                message += $"- Username changed from {user.Username} to {updatedUser.Username}\n";
                user.Username = updatedUser.Username;
            }

            if (!string.IsNullOrEmpty(updatedUser.Password) && user.Password != updatedUser.Password)
            {
                message += $"- Password changed from {user.Password} to {updatedUser.Password}\n";
                user.Password = updatedUser.Password;
            }

            if (!string.IsNullOrEmpty(updatedUser.Email) && user.Email != updatedUser.Email)
            {
                message += $"- Email changed from {user.Email} to {updatedUser.Email}\n";
                user.Email = updatedUser.Email;
            }

            if (updatedUser.Autho.HasValue && user.Autho != updatedUser.Autho.Value)
            {
                if (updatedUser.Autho.Value == 1)
                {
                    message += $"- User {user.Username} is now an admin\n";
                }
                else if (updatedUser.Autho.Value == 0)
                {
                    message += $"- User {user.Username} is no longer an admin\n";
                }

                user.Autho = updatedUser.Autho.Value;
            }

            _dbContext.SaveChanges();

            return Ok(message);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return Ok($"User {user.Username} is deleted.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
