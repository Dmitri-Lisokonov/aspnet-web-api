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
        private UserInputValidator _validator;
        private HashHelper _hashHelper;
        public UserRepository(IConfiguration config)
        {
            _jwtManager = new JWTManager(config);
            _context = new UserContext();
            _validator = new UserInputValidator();
            _hashHelper = new HashHelper();
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

        public string CreateNew(User user)
        {
            Random random = new Random();
            int length = 100;
            user.Role = "customer";
            string result;

            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                user.ConfirmationToken = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
                result = _validator.ValidateRegistrationInput(user);
                string salt = _hashHelper.GenerateSalt();
                string hash = _hashHelper.HashPassword(user.Password, salt);
                user.Password = hash;
                user.Salt = salt;
                user.Email.ToLower();
            }
            catch
            {
                return "hash failed";
            }
            if (result.Equals("success"))
            {
                bool fetch = _context.CreateNew(user);
                if (!fetch)
                {
                    return "Could not create user because of server error, please try again later";
                }
            }
            return result;
        }

        public UserViewModel Login(User user)
        {
            User fetchedUser = _context.GetByEmail(user.Email);
            if(fetchedUser != null)
            {
                string passwordHash = _hashHelper.HashPassword(user.Password, fetchedUser.Salt);
                if (fetchedUser != null && fetchedUser.Email.Equals(user.Email.ToLower()) && fetchedUser.Password.Equals(passwordHash))
                {
                    string token = _jwtManager.GenerateJSONWebToken(fetchedUser);
                    return new UserViewModel(fetchedUser.Name, fetchedUser.Email, fetchedUser.Role, fetchedUser.Verified, token);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
   
        }

        public User SendVerification(User user)
        {
            User fetchedUser = _context.GetByEmail(user.Email);
            if (!fetchedUser.Verified)
            {
                GoogleSTMP emailService = new GoogleSTMP();
                Email mail = new Email("dmitri.lisokonov@gmail.com", "Blueshop verification", $"Hello, {fetchedUser.Name} please verify your email by visiting this link: https://localhost:44350/user/verify/{fetchedUser.ConfirmationToken}");
                try
                {
                    emailService.SendEmail(mail);
                    return fetchedUser;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return fetchedUser;
            }
   
        }

        public bool VerifyEmail(string confirmationToken)
        {
            bool verified = _context.VerifyEmail(confirmationToken);

            if (verified)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
