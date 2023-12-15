using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient
{
    public class RemoteClient
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }

        public RemoteClient(string ipAddress, int port)
        {
            IPAddress = ipAddress;
            Port = port;
        }
    }
}
