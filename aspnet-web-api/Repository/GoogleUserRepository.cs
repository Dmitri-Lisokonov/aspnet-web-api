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

        public UserViewModel GetByEmail(string email)
        {
            User user = context.GetByEmail(email);
            return new UserViewModel(user.Name, user.Email);
        }

        public UserViewModel CreateNew(GoogleOAuthResult userResult)
        {
            User user = context.CreateNew(userResult);
            return new UserViewModel(user.Name, user.Email);
        }
    }
}
