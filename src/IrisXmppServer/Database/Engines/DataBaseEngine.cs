using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrisXMPPServer.Database.Engines
{
    public class DataBaseEngine
    {
        private string host;
        private int port;
        private string user;
        private string password;
        private string schema;

        public string Host
        {
            get
            {
                return host;
            }

            set
            {
                host = value;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public string User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Schema
        {
            get
            {
                return schema;
            }

            set
            {
                schema = value;
            }
        }

        public DataBaseEngine(string host, int port, string user, string password, string schema)
        {
            this.host = host;
            this.port = port;
            this.user = user;
            this.password = password;
            this.schema = schema;
        }
    }
}
