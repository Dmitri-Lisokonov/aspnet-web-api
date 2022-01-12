using aspnet_web_api.Context;
using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace aspnet_web_api.Repository
{
    public class UserRepository
    {
        private readonly UserContext _context;
        private readonly JWTManager _jwtManager;
        private readonly UserInputValidator _validator;
        private readonly HashHelper _hashHelper;
        private readonly GoogleSTMP _emailService;
        public UserRepository(IConfiguration config)
        {
            _jwtManager = new JWTManager(config);
            _context = new UserContext();
            _validator = new UserInputValidator();
            _hashHelper = new HashHelper();
            _emailService = new GoogleSTMP();
        }

        public UserViewModel GetByEmail(string email)
        {
            User user = _context.GetByEmail(email);
            if (user != null)
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
                const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
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
            if (fetchedUser != null)
            {
                string passwordHash = _hashHelper.HashPassword(user.Password, fetchedUser.Salt);
                if (fetchedUser.Email.Equals(user.Email.ToLower()) && fetchedUser.Password.Equals(passwordHash))
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

                Email mail = new Email("dmitri.lisokonov@gmail.com", "Blueshop verification", $"Hello, {fetchedUser.Name} please verify your email by visiting this link: https://blueshop.tech/activate/{fetchedUser.ConfirmationToken}");
                try
                {
                    _emailService.SendEmail(mail);
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

        public User ForgotPassword(string email)
        {
            User fetchedUser = _context.GetByEmail(email);
            Random random = new Random();
            int length = 100;
            if (fetchedUser != null)
            {
                if (fetchedUser.Verified)
                {
                    const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
                    fetchedUser.ConfirmationToken = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
                    fetchedUser.ResetDate = DateTime.Now;
                    bool result = _context.UpdateUser(fetchedUser);

                    if (result)
                    {
                        Email mail = new Email("dmitri.lisokonov@gmail.com", "Blueshop account recovery", $"Hello, {fetchedUser.Name} please reset your password by visiting this link https://blueshop.tech/reset/{fetchedUser.ConfirmationToken}");
                        _emailService.SendEmail(mail);
                        return fetchedUser;
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
            else
            {
                return null;
            }
           
        }

        public string ResetPassword(string confirmationToken, string password)
        {
            User result = _context.GetByConfirmToken(confirmationToken);
            string failed = "Invalid or expired token, please try again";
            if (result != null)
            {
                DateTime now = DateTime.Now;
                TimeSpan interval = now - result.ResetDate.ToLocalTime();
                int expiration = 5;

                if (interval.TotalMinutes < expiration)
                {
                    try
                    {
                        string passwordResult = _validator.CheckPasswordStrength(password);
                        if(passwordResult.Equals("success"))
                        {
                            result.Password = password;
                            string hash = _hashHelper.HashPassword(result.Password, result.Salt);
                            result.Password = hash;
                        }
                        else
                        {
                            return passwordResult;
                        }
                     
                    }
                    catch
                    {
                        return "hash failed";
                    }
                    bool updated = _context.UpdateUser(result);

                    if(updated)
                    {
                        return "success";
                    }
                    else
                    {
                        return failed;
                    }
                    
                }
                else
                {
                    return failed;
                }
            }
            else
            {
                return failed;
            }
        }
    }
}
