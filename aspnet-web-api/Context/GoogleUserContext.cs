using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace aspnet_web_api.Context
{
    public class GoogleUserContext
    {
        DatabaseManager manager;
        DataTableConverter converter;
        string connectionString = @"./Config/DatabaseConfig.json";
        public GoogleUserContext()
        {
            converter = new DataTableConverter();
            manager = new DatabaseManager(connectionString);
        }

        public List<UserViewModel> GetByEmail(string email)
        {

            List<UserViewModel> users = new List<UserViewModel>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", email);
            string query = $"SELECT * FROM user_google WHERE user_google.email = @email";
            DataTable table = manager.ExecuteQuery(query, true, dict);
            if (table != null)
            {
                try
                {
                    users = converter.ConvertToUserViewModel(table);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return users;
        }

        public List<UserViewModel> CreateNew(GoogleOAuthResult userResult)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", userResult.Email);
            dict.Add("@name", userResult.Name);
            string query = $"INSERT INTO user_google (email, name) VALUES(@email, @name)";
            DataTable table = manager.ExecuteQuery(query, true, dict);
            try
            {
                users = converter.ConvertToUserViewModel(table);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return users;
        }
    }
}

