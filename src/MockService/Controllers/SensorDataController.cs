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

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var sensorData = _sensorDataRepository.Get(id);
            return sensorData == null ? NotFound() : Ok(sensorData);
        }

        [HttpGet("generate-one")]
        public ActionResult GenerateOne()
        {
            var sensorData = new SensorData
            {
                SensorId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Data = DateTime.UtcNow.Ticks,
            };
            _sensorDataRepository.Save(sensorData);
            return Ok(sensorData);
        }

        [HttpGet("first")]
        public ActionResult GetFirst()
        {
            var first = _sensorDataRepository.GetFirst();
            return first == null ? NotFound() : Ok(first);
        }

        [HttpGet("last")]
        public ActionResult GetLast()
        {
            var last = _sensorDataRepository.GetLast();
            return last == null ? NotFound() : Ok(last);
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<SensorData>> GetAll()
        {
            var all = _sensorDataRepository.GetAll();
            return all == null ? NotFound() : Ok(all);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SensorData sensorData)
        {
            if (sensorData == null)
                return BadRequest("Sensor data is null.");
            
             _sensorDataRepository.Save(sensorData);
            return CreatedAtAction(nameof(GetLast), sensorData);
        }
    }
}