using System;

namespace aspnet_web_api.Utility
{
    public class MySqlConnection
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private MySql.Data.MySqlClient.MySqlConnection Connection { get; set; }
        private static MySqlConnection _instance = null;

        public static MySqlConnection Instance()
        {
            if (_instance == null)
                _instance = new MySqlConnection();
            return _instance;
        }

        public bool Open()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
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
