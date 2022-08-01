using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRTestClient.Models
{
    public class DeviceInformation
    {
        public DeviceInformation(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
