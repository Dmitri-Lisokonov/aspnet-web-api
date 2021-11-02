using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Utility
{
    public class UserRoleAuthorizationManager
    {
        public bool AuthoriseUser(HttpContext httpContext)
        {
            bool authorised = false;
            if (httpContext.User.HasClaim(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value == "customer"))
            {
                authorised = true;
            }
            return authorised;
        }

        public bool AuthenticateAdmin(HttpContext httpContext)
        {
            bool authorised = false;
            if (httpContext.User.HasClaim(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value == "admin"))
            {
                authorised = true;
            }
            return authorised;
        }
    }
}
