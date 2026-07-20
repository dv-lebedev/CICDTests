namespace MockService.Data
{
    public class SensorData
    {
        public Guid SensorId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public long Data { get; set; }
    }
}