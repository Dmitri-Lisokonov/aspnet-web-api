using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
