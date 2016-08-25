using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IrisXMPPServer.CoreClasses;
using IrisXMPPServer.Database.Engines;

namespace IrisXMPPServer.Database
{
    public class DbHandler
    {
        private DataBaseEngine dataBaseEngine;

        public DataBaseEngine DataBaseEngine
        {
            get
            {
                return dataBaseEngine;
            }

            set
            {
                dataBaseEngine = value;
            }
        }

        public DbHandler(Database.SupportedEngine engine, string host, int port, string user, string password, string schema)
        {
            if(engine == SupportedEngine.MYSQL)
            {
                dataBaseEngine = new MySqlEngine(host, port, user, password, schema);
            }
        }

        public bool UserAuthentication(string jid, string resource, string password)
        {
            if (dataBaseEngine.GetType() == typeof(MySqlEngine))
            {
                string query = "select 1 from user where jid='" + jid + "' and password='" + password + "'";
                MySql.Data.MySqlClient.MySqlDataReader reader = ((MySqlEngine)dataBaseEngine).executeQuery(query);
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader[0]) == 1)
                    {
                        return true;
                    }
                }
                ((MySqlEngine)dataBaseEngine).CloseConnection();
            }
            return false;
        }
    }
    
}
