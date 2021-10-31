using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using aspnet_web_api.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace aspnet_web_api.Controllers
{
    [ApiController]
    [Route("/user/google")]
    public class GoogleUserController : ControllerBase
    {
        private GoogleUserRepository _repo;
        private readonly ILogger<GoogleUserController> _logger;
        private GoogleOAuthHelper _authHelper;

        public GoogleUserController(ILogger<GoogleUserController> logger)
        {
            _repo = new GoogleUserRepository();
            _authHelper = new GoogleOAuthHelper();
            _logger = logger;
        }

        [HttpPost]
        [Route("{email}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetByEmail(string email, [FromBody]GoogleOAuthToken token)
        {
            List<UserViewModel> users = _repo.GetByEmail(email);
            GoogleOAuthResult result;
            try
            {
                result = _authHelper.ValidateToken(token.token).Result;
            }
            catch (Exception e)
            {
                return StatusCode(401, "User Unauthorized");
            }
            if (users.Count < 1)
            {
                //Change status code
                return StatusCode(401, "User Unauthorized");
            }
            if (users != null)
            {
                if (users[0].Email.Equals(result.Email))
                {
                    return Ok(users);
                }
                else
                {
                    return StatusCode(401, "User Unauthorized");
                }
            }

            else
            {
                return StatusCode(500, "The server was unable to process your request due to server error or invalid paramaters");
            }
        }
        
        //Todo: Consider removing
        [HttpPost]
        [Route("auth")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ValidateLogin(GoogleOAuthToken token)
        {
            GoogleOAuthResult result;
            try
            {
                result = _authHelper.ValidateToken(token.token).Result;
                return Ok(result);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(401, "User Unauthorized");
            }

        }


        [HttpPost]
        [Route("auth/register")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult RegisterUser(GoogleOAuthToken token)
        {
            GoogleOAuthResult authResult;
            try
            {
                authResult = _authHelper.ValidateToken(token.token).Result;
                List<UserViewModel> existingUser = _repo.GetByEmail(authResult.Email);
                if(existingUser.Count > 0)
                {
                    return Ok("Already registered");
                }
                else
                {
                    List<UserViewModel> result = _repo.CreateNew(authResult);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(401, "User Unauthorized");
            }
        }
    }
}
