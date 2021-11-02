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

        public User GetByEmail(string email)
        {

            List<User> user = new List<User>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", email);
            string query = $"SELECT * FROM user_google WHERE user_google.email = @email";
            DataTable table = manager.ExecuteQuery(query, true, dict);
            if (table != null)
            {
                try
                {
                    user = converter.ConvertToUser(table);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return user[0];
        }

        public User CreateNew(GoogleOAuthResult userResult)
        {
            List<User> user = new List<User>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", userResult.Email);
            dict.Add("@name", userResult.Name);
            string query = $"INSERT INTO user_google (email, name) VALUES(@email, @name)";
            DataTable table = manager.ExecuteQuery(query, true, dict);
            try
            {
                user = converter.ConvertToUser(table);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return user[0];
        }
    }
}

