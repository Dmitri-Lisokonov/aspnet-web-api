using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private UserRepository _repo;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _repo = new UserRepository();
            _logger = logger;
        }
        [HttpGet]
        [Route("all")]
        public List<UserViewModel> getAll()
        {
            return _repo.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public List<UserViewModel> getAll(int id)
        {
            return _repo.GetById(id);
        }
    }
}
