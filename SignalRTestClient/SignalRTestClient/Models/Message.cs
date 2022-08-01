using System;

namespace SignalRTestClient.Models
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
