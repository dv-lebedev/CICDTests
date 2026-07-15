namespace MockService.Data;

public interface ISensorDataRepository
{
    void Save(SensorData sensorData);
    SensorData? Get(Guid id);
    SensorData? GetLastOne();
    IEnumerable<SensorData> GetAll();
}

public class SensorDataRepository : ISensorDataRepository
{
    private const int MaxQueueSize = 10;

    private readonly Queue<SensorData> _queue = new();
    private readonly object _sync = new();

    public SensorDataRepository() 
    {
        for (int i = 0; i < MaxQueueSize; i++)
        {
            var sensorData = new SensorData
            {
                SensorId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddMinutes(-i),
                Data = i * 10
            };
            _queue.Enqueue(sensorData);
        }
    }

    public void Save(SensorData sensorData)
    {
        lock (_sync)
        {
            _queue.Enqueue(sensorData);
            if (_queue.Count > MaxQueueSize)
            {
                _queue.Dequeue();
            }
        }
    }

    public SensorData? GetLastOne()
    {
        lock (_sync)
        {
            return _queue.LastOrDefault();
        }
    }

    public IEnumerable<SensorData> GetAll()
    {
        lock (_sync)
        {
            return _queue.ToList();
        }
    }

    public SensorData? Get(Guid id)
    {
        lock (_sync)
        {
            return _queue.FirstOrDefault(x => x.SensorId == id);
        }
    }
}