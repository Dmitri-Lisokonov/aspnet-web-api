using aspnet_web_api.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Text.Json;


namespace aspnet_web_api.Utility
{
    public class DatabaseManager
    {
        private string configPath;
        MySqlDatabaseConnection database;

        public DatabaseManager(string configPath)
        {
            this.configPath = configPath;
            database = MySqlDatabaseConnection.Instance();
        }
        public bool Connect()
        {
            //Read from config and set credentials
            string json = File.ReadAllText(configPath);
            DatabaseCredentials credentials = new DatabaseCredentials();
            credentials = JsonSerializer.Deserialize<DatabaseCredentials>(json);
            database.Server = credentials.Server;
            database.DatabaseName = credentials.Database;
            database.UserName = credentials.Username;
            database.Password = credentials.Password;
            //Open connection
            bool isOpen = database.Open();
            return isOpen;
        }

        public DataTable ExecuteQuery(string sql, string parameter = null, int paramValue = 0)
        {
            DataTable table = new DataTable();
     
            try
            {
                
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = database.Connection;
                cmd.CommandText = sql;

                if (parameter != null)
                {
                    cmd.Parameters.AddWithValue(parameter, paramValue);
                    cmd.Prepare();
                    Console.WriteLine(cmd.Parameters);
                }
                MySqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                reader.Close();
                return table;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
