using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using aspnet_web_api.Models;

namespace aspnet_web_api.Utility
{

    public class GoogleOAuthHelper
    {
        public async Task<GoogleOAuthResult> ValidateToken(string token)
        {
            GoogleOAuthResult result = new GoogleOAuthResult();

            try
            {
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);
                if (!payload.Audience.Equals("159151186149-jbv6ar6d2m1v8ep15s8a82akngkd8e74.apps.googleusercontent.com"))
                    result.ValidationResult = false;
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                    result.ValidationResult = false;
                if (payload.ExpirationTimeSeconds == null)
                    result.ValidationResult = false;
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        result.ValidationResult = false;
                    }
                }
                result.Name = payload.Name;
                result.Email = payload.Email;

            }
            catch (InvalidJwtException e)
            {
                result.ValidationResult = false;
            }
            return result;
        }
    }
}
