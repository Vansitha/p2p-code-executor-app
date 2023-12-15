using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using PeerServerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopClient
{
    internal class NetworkingThread
    {
        private int ConnectedPort;
        private String IpAddress;
        private Thread NetworkThread;
        private Base64Encoder Base64Encoder;
        private readonly string webServerUrl = "http://localhost:5148/";
        private bool isRun = true;

        public NetworkingThread(int ConnectedPort, string IpAddress)
        {
            this.ConnectedPort = ConnectedPort;
            this.IpAddress = IpAddress;
            Base64Encoder = new Base64Encoder();
        }

        public async void Start()
        {
            NetworkThread = new Thread(new ThreadStart(NetworkWorker));
            NetworkThread.Start();
            // make api call and register this client
            await RegisterClientAsync();
        }

        private async void NetworkWorker()
        {
            while (isRun)
            {
                // Query the Web Service for a list of other clients
                List<RemoteClient> clients = await GetNewClientsAsync();

                foreach (RemoteClient client in clients)
                {
                    if (client.Port != ConnectedPort)
                    {
                        // Connect to the .NET Remoting server of the remote client
                        ClientServiceInterface remoteServer = ConnectToRemoteServer(client.IPAddress, client.Port);

                        if (remoteServer != null)
                        {
                            try
                            {
                                // Query if jobs exist on the remote server
                                if (remoteServer.CheckIsJobAvailabe())
                                {

                                    string encodedJobCode = "";
                                    string hash = "";
                                    // Download the job from the remote server
                                    remoteServer.GetAvailabeJob(out encodedJobCode, out hash);

                                    bool isHashVerifed = SHA256HashUtility.VerifySHA256Hash(encodedJobCode, hash);

                                    if (!String.IsNullOrEmpty(encodedJobCode) || isHashVerifed)
                                    {
                                        string decodedCode = Base64Encoder.Decode(encodedJobCode);
                                        // Execute the downloaded code using IronPython
                                        string result = ExecuteIronPythonCode(decodedCode);

                                        string encodeSolution = Base64Encoder.Encode(result);

                                        // Post the answer back to the client that hosted the job
                                        remoteServer.UploadJobSolution(encodeSolution);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Remote server timedout");
                                continue;
                            }
                        }
                    }
                }

                Thread.Sleep(3000); // Sleep before checking again
            }
        }

        private ClientServiceInterface ConnectToRemoteServer(string ipAddress, int port)
        {
            if (ipAddress.Equals(this.IpAddress)) ipAddress = "net.tcp://localhost";
            try
            {
                string remoteServerUrl = $"{ipAddress}:{port}/PeerServer";
                NetTcpBinding tcp = new NetTcpBinding();

                ChannelFactory<ClientServiceInterface> channelFactory = new ChannelFactory<ClientServiceInterface>(tcp, new EndpointAddress(remoteServerUrl));
                ClientServiceInterface remoteServer = channelFactory.CreateChannel();

                return remoteServer;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to remote server: " + ex.Message);
                return null;
            }
        }


        public async void Stop()
        {
            await UnRegisterClientAsync();
            isRun = false;
            NetworkThread.Join(); // Wait for the thread to finish
        }

        private async Task RegisterClientAsync()
        {
            try
            {
                var registrationData = new
                {
                    ipAddress = IpAddress,
                    port = ConnectedPort
                };

                string apiUrl = webServerUrl + "register";

                using (HttpClient client = new HttpClient())
                {
                    string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(registrationData);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Registration successful.");
                    }
                    else
                    {
                        Console.WriteLine("Registration failed: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async Task UnRegisterClientAsync()
        {
            try
            {
                var registrationData = new
                {
                    ipAddress = IpAddress,
                    port = ConnectedPort
                };

                string apiUrl = webServerUrl + "remove";

                using (HttpClient client = new HttpClient())
                {
                    string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(registrationData);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Removed client successful.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to remove client: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while removing a client: " + ex.Message);
            }
        }

        private async Task<List<RemoteClient>> GetNewClientsAsync()
        {
            List<RemoteClient> clients = new List<RemoteClient>();

            try
            {
                string apiUrl = webServerUrl + "peers";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        clients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RemoteClient>>(jsonContent);
                    }
                    else
                    {
                        Console.WriteLine("API request failed: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return clients;
        }
        public string ExecuteIronPythonCode(string pythonCode)
        {
            try
            {
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                engine.Execute(pythonCode, scope);
                dynamic result = scope.GetVariable("result");
                return result.ToString();
            }
            catch (Exception ex)
            {
                return "Python Error: " + ex.Message;
            }
        }
    }
}
