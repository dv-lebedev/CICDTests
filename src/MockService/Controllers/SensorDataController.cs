using Microsoft.AspNetCore.Mvc;
using MockService.Data;

namespace MockService.Controllers
{
    [ApiController]
    [Route("sensor-data")]
    public class SensorDataController : ControllerBase
    {
        private readonly ISensorDataRepository _sensorDataRepository;

        public SensorDataController(ISensorDataRepository sensorDataRepository)
        {
            _sensorDataRepository = sensorDataRepository;
        }

        [HttpGet]
        public ActionResult Get(Guid guid)
        {
            var sensorData = _sensorDataRepository.Get(guid);
            if (sensorData == null)
            {
                return NotFound();
            }
            return Ok(sensorData);
        }

        [HttpGet("oldest")]
        public SensorData? GetOldest()
        {
            return _sensorDataRepository.GetLastOne();
        }

        [HttpGet("all")]
        public IEnumerable<SensorData> GetAll()
        {
            return _sensorDataRepository.GetAll();
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _sensorDataRepository.Save(new SensorData
            {
                SensorId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Data = DateTime.UtcNow.Ticks,
            });
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] SensorData sensorData)
        {
            if (sensorData == null)
            {
                return BadRequest("Sensor data is null.");
            }
             _sensorDataRepository.Save(sensorData);
            return CreatedAtAction(nameof(GetOldest), sensorData);
        }
    }
}