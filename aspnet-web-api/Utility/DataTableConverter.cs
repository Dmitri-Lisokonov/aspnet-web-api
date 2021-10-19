using aspnet_web_api.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace aspnet_web_api.Utility
{
    public class DataTableConverter
    {
        public List<UserViewModel> ConvertToUserViewModel(DataTable dataTable)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                UserViewModel user = new UserViewModel();
                user.Id = Convert.ToInt32(dataTable.Rows[i]["id"]);
                user.Username = dataTable.Rows[i]["username"].ToString();
                users.Add(user);
            }
            return users;
        }
        public List<Product> ConvertToProduct(DataTable dataTable)
        {
            List<Product> products = new List<Product>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Product product = new Product();
                product.Id = Convert.ToInt32(dataTable.Rows[i]["id"]);
                product.Name = dataTable.Rows[i]["name"].ToString();
                product.Description = dataTable.Rows[i]["description"].ToString();
                product.Price = Convert.ToInt32(dataTable.Rows[i]["price"]);
                products.Add(product);
            }
            return products;
        }
    }
}
