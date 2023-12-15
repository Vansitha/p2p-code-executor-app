using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using PeerServerInterface;

namespace DesktopClient
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ServerThread : ClientServiceInterface
    {
        private ServiceHost host;
        private NetTcpBinding tcp;
        private Thread serverThread;
        private readonly int ConnectedPort;
        private readonly String IpAddress;
        private List<Job> JobsList;
        private List<Solution> SolutionList;
        private Base64Encoder base64Encoder;
        private TextBox resultTextBox;
        private Label ongoingTextBox;
        private Label completedTextBox;
        private Label queuedTextBox;
        private int onGoingJobCount;
        private int completedJobCount;
        private readonly string webServerUrl = "http://localhost:5148/";

        public ServerThread(int connectedPort, string IpAddress, TextBox resultTextBox, Label onGoingTextBox, Label completedTextBox, Label queuedTextBox)
        {
            this.ConnectedPort = connectedPort;
            this.IpAddress = IpAddress;
            this.JobsList = new List<Job>();
            this.SolutionList = new List<Solution>();
            this.base64Encoder = new Base64Encoder();
            this.resultTextBox = resultTextBox;
            this.ongoingTextBox = onGoingTextBox;
            this.completedTextBox = completedTextBox;
            this.queuedTextBox = queuedTextBox;
            this.onGoingJobCount = 0;
            this.completedJobCount = 0;
        }

        public void Start()
        {
            // Create a new thread to run the server
            serverThread = new Thread(new ThreadStart(ServerWorker));
            serverThread.Start();
        }

        private void ServerWorker()
        {
            StartServer();
        }

        public void Stop()
        {

            // Signal the server thread to stop
            host.Close();
            serverThread.Join(); // Wait for the thread to finish

        }

        private void StartServer()
        {
            tcp = new NetTcpBinding();
            host = new ServiceHost(this);
            host.AddServiceEndpoint(typeof(ClientServiceInterface), tcp, $"{IpAddress}:{ConnectedPort}/PeerServer");
            host.Open();
        }

        public void CreateJob(string codeBlock)
        {
            // Encode data and create hash for job
            Base64Encoder encoder = new Base64Encoder();
            var EncodedCodeBlock = encoder.Encode(codeBlock);

            string hash = SHA256HashUtility.GenerateSHA256Hash(EncodedCodeBlock);

            Job newJob = new Job(EncodedCodeBlock, hash);
            JobsList.Add(newJob);

            Application.Current.Dispatcher.Invoke(() =>
            {
                queuedTextBox.Content = "Queued: " + JobsList.Count;
            });
        }

        /* Methods for communicating with other clients */

        public bool CheckIsJobAvailabe()
        {
            try
            {
                return JobsList.Count > 0;
            }
            catch { return false; }
        }

        public void GetAvailabeJob(out string jobCodeEncoded, out string hash)
        {
            jobCodeEncoded = "";
            hash = "";

            try
            {
                if (CheckIsJobAvailabe())
                {
                    Job job = JobsList[0];
                    JobsList.RemoveAt(0);
                    jobCodeEncoded = job.JobEncoded;
                    hash = job.hash;

                    // Update ongoing job count
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        queuedTextBox.Content = "Queued: " + JobsList.Count;
                        onGoingJobCount++;
                        ongoingTextBox.Content = "Ongoing Jobs: " + onGoingJobCount;
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred");
            }
        }

        public void UploadJobSolution(string solutionEncoded)
        {
            // Once a solution is uploaded
            // Decode solution and add to solution list
            // Update GUI with the entire list again
            string solutionDecoded = base64Encoder.Decode(solutionEncoded);
            Solution solution = new Solution(solutionDecoded, "hash");
            SolutionList.Add(solution);

            Application.Current.Dispatcher.Invoke(async () =>
            {
                if (String.IsNullOrEmpty(resultTextBox.Text))
                {
                    resultTextBox.Text = solutionDecoded;
                }
                else
                {
                    resultTextBox.Text += "\n" + solutionDecoded;
                }


                // Update completed job count and ongoing job count
                onGoingJobCount--;
                ongoingTextBox.Content = "Ongoing Jobs: " + onGoingJobCount;
                completedJobCount++;
                completedTextBox.Content = "Completed Jobs: " + completedJobCount;

                // Make an api call and send the job count ip port and jobsCompleted
                await UpdateJobCompletionCountAsync();
            });
        }

        private async Task UpdateJobCompletionCountAsync()
        {
            string apiUrl = webServerUrl + "jobComplete";
            var updatedClientDetails = new
            {
                ipAddress = IpAddress,
                port = ConnectedPort,
                totalJobsCompleted = completedJobCount
            };
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(updatedClientDetails);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Update failed: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
