namespace WebServer.Models
{
    public class Client
    {
        public string? IpAddress { get; set; }
        public int Port { get; set; }
        public int TotalJobsCompleted { get; set; }
    }
}
