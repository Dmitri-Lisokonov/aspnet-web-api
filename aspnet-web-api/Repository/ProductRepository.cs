using aspnet_web_api.Context;
using aspnet_web_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Repository
{
    public class ProductRepository
    {
        ProductContext context;
        public ProductRepository()
        {
            context = new ProductContext();
        }

        public List<Product> GetAll()
        {
            return context.GetAll();
        }

        public List<Product> GetById(int id)
        {
            return context.GetById(id);
        }
    }
}
