using aspnet_web_api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace aspnet_web_api.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private UserRepository _repo;
        private readonly ILogger<UserController> _logger;
        private UserRoleAuthorizationManager _authManager;


        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _authManager = new UserRoleAuthorizationManager();
            _repo = new UserRepository(config);
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            _logger.LogInformation($"[POST] /user/register [IP] {Request.HttpContext.Connection.RemoteIpAddress}");

            try
            {
                UserViewModel fetchedUser = _repo.GetByEmail(user.Email);

                if (fetchedUser != null)
                {
                    return StatusCode(409, new ResponseMessage(ResponseType.FAILED, "Email is already taken"));
                }
                else
                {
                    string result = _repo.CreateNew(user);
                    if (result.Equals("success"))
                    {
                        return StatusCode(200, new ResponseMessage(ResponseType.SUCCESS, "Account created"));
                    }
                    else if(result.Equals("hash failed"))
                    {
                        return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Something went wrong while creating the user, please try again"));
                    }
                    else 
                    {
                        return StatusCode(400, new ResponseMessage(ResponseType.FAILED, result));
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Something went wrong, please check request body"));
            }

        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation($"[GET] /user/login [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
            try
            {
                UserViewModel result = _repo.Login(user);
                if (result != null && !result.Email.Equals("") && result.Verified)
                {
                    _logger.LogInformation($"[GET] /user/login success [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
                    return Ok(new ResponseMessage(ResponseType.SUCCESS, JsonSerializer.Serialize(result)));
                }
                else if(result != null && !result.Verified) {
                    _logger.LogInformation($"[GET] /user/login failed [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
                    return StatusCode(401, new ResponseMessage(ResponseType.FAILED, "Please verify your email before logging in"));
                }
                else
                {
                    _logger.LogInformation($"[GET] /user/login failed [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
                    return StatusCode(401, new ResponseMessage(ResponseType.FAILED, "Username or password is incorrect"));
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"[GET] /user/login failed [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
                return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Something went wrong, please check request body"));
            }
        }

        [HttpPost]
        [Route("verify")]
        public IActionResult SendVerification([FromBody] User user)
        {
            _logger.LogInformation($"[GET] /user/verify [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
            try
            {
                User fetchedUser = _repo.SendVerification(user);
                if (!fetchedUser.Verified)
                {
                    return Ok(new ResponseMessage(ResponseType.SUCCESS, "Verification email sent"));
                }
                else
                {
                    return StatusCode(409, new ResponseMessage(ResponseType.FAILED, "User is already verified"));
                }
            }
            catch
            {
                return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Could not send verification email, please check request body"));
            }
        }

        [HttpGet]
        [Route("verify/{confirmationToken}")]
        public IActionResult VerifyEmail(string confirmationToken)
        {
            _logger.LogInformation($"[GET] /user/verift/token [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
            try
            {
                bool verified = _repo.VerifyEmail(confirmationToken);
                if (verified)
                {
                    return Ok(new ResponseMessage(ResponseType.SUCCESS, "Email verified"));
                }
                else
                {
                    return Ok(new ResponseMessage(ResponseType.FAILED, "Invalid verification url or user already verified"));
                }
            }
            catch
            {
                return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Something went wrong, please try again"));
            }

        }

        [HttpGet]
        [Route("session")]
        public IActionResult VerifyUserSession()
        {
            _logger.LogInformation($"[GET] /user/session [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
            HttpContext currentUser = HttpContext;
            bool allowed = _authManager.AuthoriseUser(currentUser);
            if (allowed)
            {
                return Ok(new ResponseMessage(ResponseType.SUCCESS, "User still logged in"));
            }
            else
            {
                bool auth = _authManager.AuthoriseAdmin(currentUser);
                if(auth)
                {
                    return Ok(new ResponseMessage(ResponseType.SUCCESS, "User still logged in"));
                }
                else
                {
                    return StatusCode(401, new ResponseMessage(ResponseType.FAILED, "User Unauthorized"));
                }
                
                
            }
        }
    }
}
