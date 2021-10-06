using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Models
{
    public class Product
    {
        private int Id { get; set; }
        private string item { get; set; }
        private string Description { get; set; }
        private int price { get; set; }
    }
}
