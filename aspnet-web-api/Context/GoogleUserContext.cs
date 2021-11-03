using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace aspnet_web_api.Context
{
    public class GoogleUserContext
    {
        DatabaseManager _manager;
        DataTableConverter _converter;
        string connectionString = @"./Config/DatabaseConfig.json";
        public GoogleUserContext()
        {
            _converter = new DataTableConverter();
            _manager = new DatabaseManager(connectionString);
        }

        public User GetByEmail(string email)
        {

            List<User> users = new List<User>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", email);
            string query = $"SELECT * FROM user_google WHERE user_google.email = @email";
            DataTable table = _manager.ExecuteQuery(query, true, dict);
            if (table != null && table.Rows.Count > 0)
            {
                users = _converter.ConvertToGoogleUser(table);
                return users[0];
            }
            else
            {
                return null;
            }
        }

        public User CreateNew(User user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", user.Email);
            dict.Add("@name", user.Name);
            dict.Add("@role", user.Role);
            string query = $"INSERT INTO user_google (email, name, role) VALUES(@email, @name, @role)";
            try
            {
                DataTable table = _manager.ExecuteQuery(query, true, dict);
                if (table != null && table.Rows.Count > 0)
                {
                    List<User> createdUser = new List<User>();
                    createdUser = _converter.ConvertToUser(table);
                    return createdUser[0];
                }
                else
                {
                    return null;
                }
                    
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

