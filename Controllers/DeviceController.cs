using IT120P.Data;
using IT120P.Models;
using Microsoft.AspNetCore.Mvc;

namespace IT120P.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceDbContext _dbContext;

        public DeviceController(DeviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("add")]
        public IActionResult AddDevice([FromBody] Devices device)
        {
            // set default status to 1 or Online
            device.Status = 1;

            // add device to database
            _dbContext.Devices.Add(device);
            _dbContext.SaveChanges();

            var deviceName = $"{device.Brand} {device.Model}";

            return Ok($"Device '{deviceName}' added");
        }

        [HttpGet("listall")]
        public IActionResult GetAllDevices()
        {
            var devices = _dbContext.Devices.ToList();
            if (devices.Count == 0)
            {
                return Ok("No devices");
            }
            else
            {
                return Ok(devices);
            }
        }

        [HttpPut("changestatus/{id}")]
        public IActionResult ChangeDeviceStatus(int id)
        {
            var device = _dbContext.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            var deviceName = $"{device.Brand} {device.Model}";

            if (device.Status == 1)
            {
                device.Status = 0;
                _dbContext.SaveChanges();
                return Ok($"The {deviceName} is now Offline.");
            }
            else
            {
                device.Status = 1;
                _dbContext.SaveChanges();
                return Ok($"The {deviceName} is now Online.");
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDevice(int id)
        {
            // delete device from database
            var device = _dbContext.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            var deviceName = $"{device.Brand} {device.Model}";

            _dbContext.Devices.Remove(device);
            _dbContext.SaveChanges();

            return Ok($"Device '{deviceName}' removed");
        }
    }
}
