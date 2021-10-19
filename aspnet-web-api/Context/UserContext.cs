using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace aspnet_web_api.Context
{
    public class UserContext
    {
        DatabaseManager manager;
        DataTableConverter converter;
        string connectionString = @"./Config/DatabaseConfig.json";
        public UserContext()
        {
            converter = new DataTableConverter();
            manager = new DatabaseManager(connectionString);
        }

        public List<UserViewModel> GetAll()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            string query = "SELECT * FROM User";
            DataTable table = manager.ExecuteQuery(query);
            try
            {
                users = converter.ConvertToUserViewModel(table);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return users;
        }

        public List<UserViewModel> GetById(int id)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            string paramater = "@id";
            string query = "SELECT * FROM User WHERE User.id =" + paramater;
            DataTable table = manager.ExecuteQuery(query, paramater, id);
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
