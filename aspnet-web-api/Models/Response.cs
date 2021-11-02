using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Models
{
    public class Response
    {
        public ResponseType Status { get; set; }
        public string Message { get; set; }

        public Response(ResponseType status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
