namespace MockService.Data;

public interface ISensorDataRepository
{
    SensorData? Get(Guid id);
    SensorData? GetFirst();
    SensorData? GetLast();
    IEnumerable<SensorData> GetAll();
    void Save(SensorData sensorData);
}

public class SensorDataRepository : ISensorDataRepository
{
    private readonly SensorDataContext _context;
    private readonly object _sync = new();

    public SensorDataRepository(SensorDataContext context) 
    {
        _context = context;
    }

    public void Save(SensorData sensorData)
    {
        lock (_sync)
        {
            _context.Save(sensorData);
        }
    }

    public SensorData? GetFirst()
    {
        lock (_sync)
        {
            return _context.GetFirst();
        }
    }

    public SensorData? GetLast()
    {
        lock (_sync)
        {
            return _context.GetLast();
        }
    }

    public IEnumerable<SensorData> GetAll()
    {
        lock (_sync)
        {
            return _context.GetAll();
        }
    }

    public SensorData? Get(Guid id)
    {
        lock (_sync)
        {
            return _context.Get(id);
        }
    }
}