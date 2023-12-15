using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkingThread networkingThread;
        ServerThread serverThread;

        int totalJobsCompleted;
        int totalOnGoingJobs;
        int connectedPort;
        string IpAddress = "net.tcp://0.0.0.0";

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;

            PortSelector portSelector = new PortSelector();
            connectedPort = portSelector.SelectPeerPort();
            networkingThread = new NetworkingThread(connectedPort, IpAddress);
            serverThread = new ServerThread(connectedPort, IpAddress, ResultTextBox, OngoingTextBox, CompletedTextBox, QueuedTextBox);

            networkingThread.Start();
            serverThread.Start();

            DisplayConnectedPort();
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("this worksssssssssssssssss");
            networkingThread.Stop();
            serverThread.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string codeBlock = CodeTextBox.Text;

            if (String.IsNullOrEmpty(codeBlock))
            {
                MessageBox.Show("No code provided to execute.");
                return;
            }

            // call server thread method and add to job board
            serverThread.CreateJob(codeBlock);
        }

        private void DisplayConnectedPort()
        {
            ConnectedPort.Content = "Connected Port: " + connectedPort;
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = @"How to execute Python code:
    
            1. You need to create a 'result' variable to store the computation.
    
            For example:
    
            def add(num1, num2):
                return num1 + num2
    
            result = add(10, 20)
    
            2. Note that print statements won't work. If you want to display something, store the string in 'result'.
    
            For example:
    
            result = 'hello'
    
            This will work.
    
            3. This won't work:
    
            print('hello')";

            MessageBox.Show(message, "Python Code Execution Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
