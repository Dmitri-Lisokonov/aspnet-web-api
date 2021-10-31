using aspnet_web_api.Context;
using aspnet_web_api.Models;
using System.Collections.Generic;


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
    }
}
