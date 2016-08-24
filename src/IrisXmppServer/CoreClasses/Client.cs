using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IrisXMPPServer.CoreClasses
{
    public class Client
    {
        private string ipAddress;

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        private List<string> streamStack;

        public List<string> StreamStack
        {
            get
            {
                return streamStack;
            }

            set
            {
                streamStack = value;
            }
        }

        private string internalId;

        public string InternalId
        {
            get
            {
                return internalId;
            }

            set
            {
                internalId = value;
            }
        }

        private TcpClient tcpClient;

        public TcpClient TcpClient
        {
            get
            {
                return tcpClient;
            }

            set
            {
                tcpClient = value;
            }
        }

        private bool connected;

        public bool Connected
        {
            get
            {
                return connected;
            }

            set
            {
                connected = value;
            }
        }


        public Client(string interalId, TcpClient tcpClient)
        {
            this.internalId = interalId;
            this.streamStack = new List<string>();
            this.tcpClient = tcpClient;
        }
    }
}
