using aspnet_web_api.Context;
using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace aspnet_web_api.Repository
{
    public class GoogleUserRepository
    {
        private GoogleUserContext _context;
        private JWTManager _jwtManager;
        private GoogleOAuthHelper _authHelper;
        public GoogleUserRepository(IConfiguration config)
        {
            _jwtManager = new JWTManager(config);
            _context = new GoogleUserContext();
            _authHelper = new GoogleOAuthHelper();
        }

        public UserViewModel GetByEmail(string email)
        {
            User user = _context.GetByEmail(email);
            return new UserViewModel(user.Name, user.Email, user.Role);
        }

        public UserViewModel CreateNew(User user)
        {
            user.Role = "customer";
            User createdUser = _context.CreateNew(user);
            return new UserViewModel(createdUser.Name, createdUser.Email, user.Role);

        }

        public UserViewModel Authenticate(string tokenId)
        {
            GoogleOAuthResult result = _authHelper.ValidateToken(tokenId).Result;
            if (result.ValidationResult)
            {
                User fetchedUser = _context.GetByEmail(result.Email);
                if (fetchedUser != null)
                {
                    string token = _jwtManager.GenerateJSONWebToken(fetchedUser);
                    return new UserViewModel(fetchedUser.Name, fetchedUser.Email, fetchedUser.Role, fetchedUser.Verified, token);
                }
                else
                {
                    User user = new User();
                    user.Email = result.Email;
                    user.Name = result.Name;
                    UserViewModel createdUser = CreateNew(user);
                    if (createdUser != null)
                    {
                        string token = _jwtManager.GenerateJSONWebToken(fetchedUser);
                        return new UserViewModel(createdUser.Email, createdUser.Name, token);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            else
            {
                return null;
            }

        }
    }
}
