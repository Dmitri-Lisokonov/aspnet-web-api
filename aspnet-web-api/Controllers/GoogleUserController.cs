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
  
    }
}
