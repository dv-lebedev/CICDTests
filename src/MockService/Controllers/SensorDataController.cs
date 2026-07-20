using Microsoft.AspNetCore.Mvc;
using MockService.Data;

namespace MockService.Controllers;

[ApiController]
[Route("sensor-data")]
public class SensorDataController : ControllerBase
{
    private readonly ISensorDataRepository _sensorDataRepository;

    public SensorDataController(ISensorDataRepository sensorDataRepository)
    {
        _sensorDataRepository = sensorDataRepository;
    }

    [HttpGet("{id:guid}")]
    public ActionResult<SensorData> GetById(Guid id)
    {
        var sensorData = _sensorDataRepository.Get(id);
        return sensorData is null ? NotFound() : Ok(sensorData);
    }

    [HttpPost("generate-one")]
    public ActionResult<SensorData> GenerateOne()
    {
        var sensorData = CreateGeneratedSensorData();

        _sensorDataRepository.Save(sensorData);

        return CreatedAtAction(nameof(GetById), new { id = sensorData.SensorId }, sensorData);
    }

    [HttpGet("first")]
    public ActionResult<SensorData> GetFirst()
    {
        var first = _sensorDataRepository.GetFirst();
        return first is null ? NotFound() : Ok(first);
    }

    [HttpGet("last")]
    public ActionResult<SensorData> GetLast()
    {
        var last = _sensorDataRepository.GetLast();
        return last is null ? NotFound() : Ok(last);
    }

    [HttpGet]
    [HttpGet("all")]
    public ActionResult<IReadOnlyCollection<SensorData>> GetAll()
    {
        var all = _sensorDataRepository.GetAll().ToList();
        return Ok(all);
    }

    [HttpPost]
    public ActionResult<SensorData> Post([FromBody] SensorData? sensorData)
    {
        if (sensorData is null)
        {
            return BadRequest("Sensor data is null.");
        }

        if (sensorData.SensorId == Guid.Empty)
        {
            sensorData.SensorId = Guid.NewGuid();
        }

        if (sensorData.Timestamp == default)
        {
            sensorData.Timestamp = DateTimeOffset.UtcNow;
        }

        _sensorDataRepository.Save(sensorData);

        return CreatedAtAction(nameof(GetById), new { id = sensorData.SensorId }, sensorData);
    }

    private static SensorData CreateGeneratedSensorData()
    {
        return new SensorData
        {
            SensorId = Guid.NewGuid(),
            Timestamp = DateTimeOffset.UtcNow,
            Data = DateTime.UtcNow.Ticks
        };
    }
}