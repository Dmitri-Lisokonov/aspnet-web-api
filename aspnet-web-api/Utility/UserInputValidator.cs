using aspnet_web_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aspnet_web_api.Utility
{
    public class UserInputValidator
    {
        string[] topLevel = { ".net", ".org", ".io", ".com", ".nl", ".ru", ".ir", ".in", ".uk", ".au", ".ua", ".de" };
        private bool ValidateEmail(User user)
        {
            bool containsDomain = false;
            if (user.Email.Contains("@"))
            {
                foreach (string i in topLevel)
                {
                    if(!containsDomain)
                    {
                        if (user.Email.Contains(i))
                        {
                            containsDomain = true;
                        }
                        else
                        {
                            containsDomain = false;
                        }
                    }
                }
                return containsDomain;
            }
            else
            {
                return containsDomain;
            }
        }

        private bool CheckEmptyFields(User user)
        {
            bool noEmpty = true;

            if (user.Email.Length == 0 || user.Name.Length == 0 || user.Password.Length == 0)
            {
                noEmpty = false;
            }

            return noEmpty;
        }

        public string CheckPasswordStrength(string password)
        {
            Regex rgx = new Regex("[^A-Za-z0-9]");
            bool hasSpecialChars = rgx.IsMatch(password);
            bool hasNumbers = password.Any(char.IsDigit);
            bool hasCapital = password.Any(char.IsUpper);
            if (password.Length < 10)
            {
                return "Password needs to be atleast 10 characters long";
            }
            else if(!hasSpecialChars)
            {
                return "Password needs to contain atleast one special character";
            }
            else if (!hasNumbers)
            {
                return "Password needs to contain atleast one number";
            }
            else if (!hasCapital)
            {
                return "Password needs to contain atleast one capital letter";
            }
            else
            {
                return "success";
            }
        }

        public string ValidateRegistrationInput(User user)
        {
            string passwordStrength = CheckPasswordStrength(user.Password);
            if (!ValidateEmail(user))
            {
                return "Please enter a correct email address";
            }
            else if(!CheckEmptyFields(user))
            {
                return "All fields are required";
            }
            else if (!passwordStrength.Equals("success"))
            {
                return passwordStrength;
            }
            else
            {
                return "success";
            }
           
        }
    }
}
