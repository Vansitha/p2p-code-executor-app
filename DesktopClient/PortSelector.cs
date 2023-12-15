using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient
{
    internal class PortSelector
    {
        public int SelectPeerPort()
        {
            // Generate a random port number within a specific range
            Random random = new Random();
            int minPort = 1024;  // The minimum port number
            int maxPort = 9000; // The maximum port number

            while (true)
            {
                int candidatePort = random.Next(minPort, maxPort + 1);
                if (IsPortAvailable(candidatePort))
                {
                    return candidatePort;
                }
            }
        }

        private bool IsPortAvailable(int port)
        {
            try
            {
                // Try to bind to the specified port
                using (var client = new System.Net.Sockets.TcpClient("127.0.0.1", port))
                {
                    // If the binding is successful, the port is in use
                    return false;
                }
            }
            catch
            {
                // An exception was thrown, which means the port is available
                return true;
            }
        }
    }
}
