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
            Console.WriteLine("context");
            Console.WriteLine(email);
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

        public User GetByConfirmToken(string token)
        {

            List<User> users = new List<User>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@confirmation_token", token);
            string query = $"SELECT * FROM user WHERE user.confirmation_token = @confirmation_token";
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
            dict.Add("@salt", user.Salt);
            dict.Add("@role", user.Role);
            dict.Add("@confirmation_token", user.ConfirmationToken);
            string query = $"INSERT INTO user (email, name, password, salt, role, confirmation_token) VALUES(@email, @name, @password, @salt, @role, @confirmation_token)";
            try
            {
                DataTable table = _manager.ExecuteQuery(query, true, dict);
                if(table != null)
                {
                    return true;
                }
               else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool VerifyEmail(string confirmationToken)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@confirmation_token", confirmationToken);
            string query = $"SELECT * FROM user WHERE user.confirmation_token = @confirmation_token AND verified = 0";
            string update = $"UPDATE user SET verified = 1 WHERE confirmation_token = @confirmation_token";
            try
            {
                DataTable check = _manager.ExecuteQuery(query, true, dict);
                if(check.Rows.Count > 0)
                {
                    DataTable table = _manager.ExecuteQuery(update, true, dict);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("@email", user.Email);
            dict.Add("@name", user.Name);
            dict.Add("@password", user.Password);
            dict.Add("@salt", user.Salt);
            dict.Add("@role", user.Role);
            dict.Add("@confirmation_token", user.ConfirmationToken);
            dict.Add("@reset_date", (user.ResetDate - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString());
            string query = $"UPDATE user SET email=@email, name=@name, password=@password, salt=@salt, role=@role, confirmation_token=@confirmation_token, reset_date=@reset_date WHERE id=" + user.Id;
            try
            {
                DataTable table = _manager.ExecuteQuery(query, true, dict);
                if (table != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
