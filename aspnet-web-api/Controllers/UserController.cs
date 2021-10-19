using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult getAll()
        {
            List<UserViewModel> users = _repo.GetAll();
            if (users.Count < 1)
            {
                return NotFound("No user found with that id");
            }
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return StatusCode(500, "The server was unable to process your request due to server error or invalid paramaters");
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult getAll(int id)
        {
            List<UserViewModel> users = _repo.GetById(id);
            if (users.Count < 1)
            {
                return NotFound("No user found with that id");
            }
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return StatusCode(500, "The server was unable to process your request due to server error or invalid paramaters");
            }
        }
    }
}
