using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Models
{
    public enum ResponseType
    {
        Success,
        Failed,
        Unauthorized,
        ServerError
    }
}
