using aspnet_web_api.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace aspnet_web_api.Utility
{
    public class DataTableConverter
    {
        public List<User> ConvertToUser(DataTable dataTable)
        {
            List<User> users = new List<User>();
            if (dataTable != null)
            {
                try
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        User user = new User();
                        user.Id = Convert.ToInt32(dataTable.Rows[i]["id"]);
                        user.Email = dataTable.Rows[i]["email"].ToString();
                        user.Name = dataTable.Rows[i]["name"].ToString();
                        user.Password = dataTable.Rows[i]["password"].ToString();
                        user.Role = dataTable.Rows[i]["role"].ToString();
                        users.Add(user);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return users;
        }

        public List<User> ConvertToGoogleUser(DataTable dataTable)
        {
            List<User> users = new List<User>();
            if (dataTable != null)
            {
                try
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        User user = new User();
                        user.Id = Convert.ToInt32(dataTable.Rows[i]["id"]);
                        user.Email = dataTable.Rows[i]["email"].ToString();
                        user.Name = dataTable.Rows[i]["name"].ToString();
                        user.Role = dataTable.Rows[i]["role"].ToString();
                        users.Add(user);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return users;
        }
        public List<Product> ConvertToProduct(DataTable dataTable)
        {
            List<Product> products = new List<Product>();
            if (dataTable != null)
            {
                try
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Product product = new Product();
                        product.Id = Convert.ToInt32(dataTable.Rows[i]["id"]);
                        product.Name = dataTable.Rows[i]["name"].ToString();
                        product.Description = dataTable.Rows[i]["description"].ToString();
                        product.Price = Convert.ToInt32(dataTable.Rows[i]["price"]);
                        products.Add(product);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return products;
        }
    }
}
