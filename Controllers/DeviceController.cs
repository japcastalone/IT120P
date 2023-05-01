using IT120P.Models;
using IT120P.Services;
using Microsoft.AspNetCore.Mvc;

namespace IT120P.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceDbContext _dbContext;
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public DeviceController(DeviceDbContext dbContext, AuthService authService, UserService userService)
        {
            _dbContext = dbContext;
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (_authService.Authenticate(loginRequest.Username, loginRequest.Password))
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("devices")]
        public IActionResult AddDevice([FromBody] Devices device)
        {
            // get user id from token
            int userId = int.Parse(User.Identity.Name);

            // check if user has sufficient authority
            var user = _userService.GetById(userId);
            if (user.Autho != "admin")
            {
                return Unauthorized();
            }

            // add device to database
            _dbContext.Devices.Add(device);
            _dbContext.SaveChanges();

            return Ok(device);
        }

        [HttpDelete("devices/{id}")]
        public IActionResult DeleteDevice(int id)
        {
            // get user id from token
            int userId = int.Parse(User.Identity.Name);

            // check if user has sufficient authority
            var user = _userService.GetById(userId);
            if (user.Autho != "admin")
            {
                return Unauthorized();
            }

            // delete device from database
            var device = _dbContext.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }
            _dbContext.Devices.Remove(device);
            _dbContext.SaveChanges();

            return Ok();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
