using NetCoreXmppServer.CoreClasses;
using NetCoreXmppServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreXmppServer.Threads
{
    public class MainConnectionThread : BaseThread
    {
        private TcpListener tcpListener;
        private ConnectionState connectionState;
        public static List<Client> clients = new List<Client>();

        public ConnectionState getConnectionState()
        {
            return connectionState;
        }

        public MainConnectionThread() : base()
        {
            connectionState = new ConnectionState();
        }

        private async Task<TcpListener> startTcpListener()
        {
            TcpListener tcpListener;
            string output = "";
            try
            {
                IPAddress[] ipAddresses = (await Dns.GetHostEntryAsync("localhost")).AddressList;
                // Set the listener on the local IP address 
                // and specify the port.
                foreach(IPAddress add in ipAddresses)
                {
                    Console.WriteLine(add);
                }
                IPAddress ip = IPAddress.Parse("192.168.0.11");
                tcpListener = new TcpListener(ip, 13);
                tcpListener.Start();
                output = "Waiting for a connection...@" + tcpListener.LocalEndpoint.ToString();
                Console.WriteLine(output);
                connectionState.Started = true;
                connectionState.Status = "OK";
                return tcpListener;
            }
            catch (Exception e)
            {
                output = "Error: " + e.ToString();
                Console.WriteLine(output);
                throw e;
            }
        }

        private async Task<TcpClient> acceptTcpClient()
        {
            return (await tcpListener.AcceptTcpClientAsync());
        }

        public override void RunThread()
        {
            TcpClientStart();
        }

        public class ConnectionState
        {
            private bool started;

            public bool Started
            {
                get
                {
                    return started;
                }

                set
                {
                    started = value;
                }
            }

            public string Status
            {
                get
                {
                    return status;
                }

                set
                {
                    status = value;
                }
            }

            private string status;
        }

        private void TcpClientStart()
        {
            try
            {
                int port = 13;
                IPAddress ip = IPAddress.Parse("192.168.0.11");
                TcpListener serverSocket = new TcpListener(ip, port);
                TcpClient clientSocket = default(TcpClient);

                serverSocket.Start();
                tcpListener = serverSocket;
                Console.WriteLine("=>DotNetXmppServer Started... Listening@" + ip.ToString() + ":" + port);
                this.connectionState = new ConnectionState();
                connectionState.Started = true;
                connectionState.Status = "OK";

                while (true && this.connectionState.Started)
                {
                    clientSocket = AcceptClient().Result;
                    handleClient client = new handleClient();
                    client.startClient(clientSocket);
                }

                clientSocket.Dispose();
                serverSocket.Stop();
                Console.WriteLine("=>" + "Server Offline");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public Task<TcpClient> AcceptClient()
        {
            return tcpListener.AcceptTcpClientAsync();
        }

        //Class to handle each client request separatly
        public class handleClient
        {
            TcpClient clientSocket;
            string clNo;
            public void startClient(TcpClient inClientSocket)
            {
                this.clientSocket = inClientSocket;
                Thread ctThread = new Thread(doChat);
                ctThread.Start();
            }

            private void doChat()
            {
                Client newClient = new Client(DateTime.Now.ToString("ddMMyyyyHHmmss") + new Random().Next(5000), clientSocket);
                Console.WriteLine("=>Client Id: " + newClient.InternalId + " Connected");
                newClient.Connected = true;
                clients.Add(newClient);
                               
                SocketHelper helper = new SocketHelper(newClient);

                while (newClient.Connected)
                {
                    helper.processMsg();
                }
            }
        }
    }
}
