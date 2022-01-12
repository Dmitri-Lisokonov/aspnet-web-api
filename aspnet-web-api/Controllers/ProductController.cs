using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using aspnet_web_api.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace aspnet_web_api.Controllers
{
    [Route("/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _repo;
        private ILogger<ProductController> _logger;
        private readonly UserRoleAuthorizationManager _authManager;

        public ProductController(ILogger<ProductController> logger)
        {
            _repo = new ProductRepository();
            _logger = logger;
            _authManager = new UserRoleAuthorizationManager();
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"[GET] /product/all [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
            HttpContext currentUser = HttpContext;
            try
            {
                List<Product> products = _repo.GetAll();
                if (products.Count < 1)
                {
                    return NotFound("No products found");
                }
                else
                {
                    return Ok(new ResponseMessage(ResponseType.SUCCESS, JsonSerializer.Serialize(products)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new ResponseMessage(ResponseType.FAILED, "Something went wrong, please contact server admin"));
            }

        }

        [HttpGet]
        [Authorize]
        [Route("admin")]
        public IActionResult GetSecretProduct()
        {
            _logger.LogInformation($"[GET] /product/admin [IP] {Request.HttpContext.Connection.RemoteIpAddress}");
            HttpContext currentUser = HttpContext;
            bool allowed = _authManager.AuthoriseAdmin(currentUser);
            if (allowed)
            {
                return Ok(new ResponseMessage(ResponseType.SUCCESS, "Do you want to drop all tables?"));
            }
            else
            {
                return StatusCode(401, new ResponseMessage(ResponseType.FAILED, "User Unauthorized"));
            }
        }
    }
}
