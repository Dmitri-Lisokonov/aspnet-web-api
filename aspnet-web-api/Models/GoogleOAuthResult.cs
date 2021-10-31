using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Models
{
    public class GoogleOAuthResult
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public bool ValidationResult { get; set; }
    }
}
