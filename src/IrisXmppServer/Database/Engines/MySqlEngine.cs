using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace IrisXMPPServer.Database.Engines
{
    public class MySqlEngine : DataBaseEngine
    {
        private string mySqlConnectionString;
        private MySqlConnection mysqlConnection;

        public MySqlEngine(string host, int port, string user, string password, string schema) : base(host, port, user, password, schema)
        {
            mySqlConnectionString = "server=" + host + ";port=" + port + ";uid=" + user + ";pwd=" + password + ";database=" + schema + ";SslMode=None";
        }

        public void OpenConnection()
        {
            try
            {

                mysqlConnection = new MySql.Data.MySqlClient.MySqlConnection(this.mySqlConnectionString);
                mysqlConnection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("=>MySQL Error : " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                mysqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("=>MySQL Error : " + ex.Message);
            }
        }

        public MySqlDataReader executeQuery(string query)
        {
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = mysqlConnection;
                cmd.CommandType = CommandType.Text;
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("=>MySQL Error : " + ex.Message);
            }
            return null;
        }

        public void executeQueries(List<string> queries)
        {
            try
            {
                OpenConnection();
                MySqlTransaction transaction = mysqlConnection.BeginTransaction();
                foreach(string query in queries) { 
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = query;
                    cmd.Connection = mysqlConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("=>MySQL Error : " + ex.Message);
            }
        }
    }
}
