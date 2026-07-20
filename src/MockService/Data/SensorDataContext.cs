namespace MockService.Data
{
    public class SensorDataContext
    {
        private const int MaxQueueSize = 10;
        private readonly Queue<SensorData> _queue = new();

        public SensorDataContext()
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
            _queue.Enqueue(sensorData);
            if (_queue.Count > MaxQueueSize)
            {
                _queue.Dequeue();
            }
        }

        public SensorData? GetFirst()
        {
            return _queue.FirstOrDefault();
        }

        public SensorData? GetLast()
        {
            return _queue.LastOrDefault();
        }

        public IEnumerable<SensorData> GetAll()
        {
            return _queue.ToList();
        }
        public SensorData? Get(Guid id)
        {
            return _queue.FirstOrDefault(x => x.SensorId == id);
        }
    }
}
