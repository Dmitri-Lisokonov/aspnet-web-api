using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace aspnet_web_api.Context
{
    public class ProductContext
    {
        DatabaseManager manager;
        DataTableConverter converter;
        string connectionString = @"./Config/DatabaseConfig.json";
        public ProductContext()
        {
            converter = new DataTableConverter();
            manager = new DatabaseManager(connectionString);
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM Product";
            DataTable table = manager.ExecuteQuery(query, false);
            try
            {
                products = converter.ConvertToProduct(table);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return products;
        }
    }
}
