using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using aspnet_web_api.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace aspnet_web_api.Controllers
{
    [ApiController]
    [Route("/user/google")]
    public class GoogleUserController : ControllerBase
    {
        private GoogleUserRepository _repo;
        private readonly ILogger<GoogleUserController> _logger;
        private GoogleOAuthHelper _authHelper;

        public GoogleUserController(ILogger<GoogleUserController> logger, IConfiguration config)
        {
            _repo = new GoogleUserRepository(config);
            _authHelper = new GoogleOAuthHelper();
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Authenticate([FromBody] string tokenId)
        {
            try
            {
                UserViewModel result = _repo.Authenticate(tokenId);
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
