using aspnet_web_api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using aspnet_web_api.Models;
using aspnet_web_api.Utility;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace aspnet_web_api.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private UserRepository _repo;
        private readonly ILogger<UserController> _logger;


        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _repo = new UserRepository(config);
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {

            try
            {
                UserViewModel fetchedUser = _repo.GetByEmail(user.Email);
                if (fetchedUser != null)
                {
                    return StatusCode(409, new ResponseMessage(ResponseType.FAILED, "Email is already taken"));
                }
                else
                {
                    bool result = _repo.CreateNew(user);
                    if (result)
                    {
                        return StatusCode(200, new ResponseMessage(ResponseType.SUCCESS, "Account created"));
                    }
                    else
                    {
                        return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "User could not be inserted into database"));
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
            try
            {
                UserViewModel result = _repo.Login(user);
                if (result != null && !result.Email.Equals(""))
                {
                    return Ok(new ResponseMessage(ResponseType.SUCCESS, JsonSerializer.Serialize(result)));
                }
                else
                {
                    return StatusCode(401, new ResponseMessage(ResponseType.FAILED, "Username or password is incorrect"));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Something went wrong, please check request body"));
            }
        }
    }
}
