using aspnet_web_api.Context;
using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Repository
{
    public class UserRepository
    {
        private UserContext _context;
        private JWTManager _jwtManager;
        public UserRepository(IConfiguration config)
        {
            _jwtManager = new JWTManager(config);
            _context = new UserContext();
        }

        public UserViewModel GetByEmail(string email)
        {
            User user = _context.GetByEmail(email);
            if(user != null)
            {
                return new UserViewModel(user.Name, user.Email, user.Role);
            }
            else
            {
                return null;
            }
        }

        public bool CreateNew(User user)
        {
            user.Role = "customer";
            return _context.CreateNew(user);
        }

        public UserViewModel Login(User user)
        {
            User fetchedUser = _context.GetByEmail(user.Email);
            if(fetchedUser != null && fetchedUser.Email.Equals(user.Email) && fetchedUser.Password.Equals(user.Password))
            {
                string token = _jwtManager.GenerateJSONWebToken(fetchedUser);
                return new UserViewModel(fetchedUser.Name, fetchedUser.Email, fetchedUser.Role, token);
            }
            else
            {
                return null;
            }
        }
    }
}
