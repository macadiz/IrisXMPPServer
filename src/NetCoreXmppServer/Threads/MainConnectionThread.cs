using NetCoreXmppServer.CoreClasses;
using NetCoreXmppServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            try
            {
                tcpListener = startTcpListener().Result;
            }
            catch (Exception ex)
            {
                connectionState.Started = false;
                connectionState.Status = "Error starting Listener : " + ex.Message;
            }
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
                IPAddress ip = IPAddress.Parse("192.168.0.6");
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
            
            while (true && connectionState.Started)
            {
                Thread clientThread = new Thread(ClientMessageProcess);
                clientThread.Start();
                this.Sleep(1000);
            }
        }

        private void ClientMessageProcess()
        {
            TcpClient tcpClient = acceptTcpClient().Result;
            var client = tcpClient;
            Client newClient = new Client(DateTime.Now.ToString("ddMMyyyyHHmmss") + new Random().Next(5000), client);
            newClient.Connected = true;
            clients.Add(newClient);
            // Read the data stream from the client.                 
            SocketHelper helper = new SocketHelper(newClient);

            if (newClient.Connected)
            {
                helper.processMsg();
            }
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
    }
}
