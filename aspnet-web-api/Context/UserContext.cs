using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Context
{
    public class UserContext
    {
        private DatabaseManager _manager;
        private DataTableConverter _converter;
        string connectionString = @"./Config/DatabaseConfig.json";
        public UserContext()
        {
            _converter = new DataTableConverter();
            _manager = new DatabaseManager(connectionString);
        }

        public User GetByEmail(string email)
        {

            List<User> users = new List<User>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", email);
            string query = $"SELECT * FROM user WHERE user.email = @email";
            DataTable table = _manager.ExecuteQuery(query, true, dict);
            if (table != null && table.Rows.Count > 0)
            {
                users = _converter.ConvertToUser(table);
                return users[0];
            }
            else
            {
                return null;
            }
        }

        public bool CreateNew(User user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", user.Email);
            dict.Add("@name", user.Name);
            dict.Add("@password", user.Password);
            dict.Add("@role", user.Role);
            string query = $"INSERT INTO user (email, name, password, role) VALUES(@email, @name, @password, @role)";
            try
            {
                DataTable table = _manager.ExecuteQuery(query, true, dict);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
