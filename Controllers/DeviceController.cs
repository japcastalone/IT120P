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

        public DeviceController(DeviceDbContext dbContext, AuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
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

        [HttpPost]
        [Route("")]
        public IActionResult AddDevice([FromBody] Devices device)
        {
            if (_authService.Authenticate(HttpContext.User.Identity.Name, HttpContext.Request.Headers["Authorization"]))
            {
                _dbContext.Devices.Add(device);
                _dbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteDevice(int id)
        {
            if (_authService.Authenticate(HttpContext.User.Identity.Name, HttpContext.Request.Headers["Authorization"]))
            {
                var device = _dbContext.Devices.SingleOrDefault(d => d.Id == id);
                if (device != null)
                {
                    _dbContext.Devices.Remove(device);
                    _dbContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
