using aspnet_web_api.Context;
using aspnet_web_api.Models;
using System.Collections.Generic;


namespace aspnet_web_api.Repository
{
    public class GoogleUserRepository
    {
        GoogleUserContext context;
        public GoogleUserRepository()
        {
            context = new GoogleUserContext();
        }

        public List<UserViewModel> GetByEmail(string email)
        {
            return context.GetByEmail(email);
        }

        public List<UserViewModel> CreateNew(GoogleOAuthResult userResult)
        {
            return context.CreateNew(userResult);
        }
    }
}
