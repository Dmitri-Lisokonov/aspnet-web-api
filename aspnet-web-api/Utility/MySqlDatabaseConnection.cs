using System;

namespace aspnet_web_api.Utility
{
    public class MySqlDatabaseConnection
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public MySql.Data.MySqlClient.MySqlConnection Connection { get; set; }
        private static MySqlDatabaseConnection _instance = null;

        private MySqlDatabaseConnection() { }
        public static MySqlDatabaseConnection Instance()
        {
            if (_instance == null)
                _instance = new MySqlDatabaseConnection();
            return _instance;
        }

        public bool Open()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID=root; password={2}; persistsecurityinfo=True; SslMode=required", Server, DatabaseName, Password);
                Connection = new MySql.Data.MySqlClient.MySqlConnection(connstring);
                Connection.Open();
            }

            return true;
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
