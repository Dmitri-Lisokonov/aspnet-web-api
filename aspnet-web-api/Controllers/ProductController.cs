using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using aspnet_web_api.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace aspnet_web_api.Controllers
{
    [Route("/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductRepository _repo;
        private readonly ILogger<ProductController> _logger;
        private UserRoleAuthorizationManager _authManager;

        public ProductController(ILogger<ProductController> logger)
        {
            _repo = new ProductRepository();
            _logger = logger;
            _authManager = new UserRoleAuthorizationManager();
        }

        [HttpGet]
        [Route("all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            HttpContext currentUser = HttpContext;
            bool allowed = _authManager.AuthoriseUser(currentUser);
            try
            {
                if (allowed)
                {
                    List<Product> products = _repo.GetAll();
                    if (products.Count < 1)
                    {
                        return NotFound("No products found");
                    }
                    else
                    {
                        return Ok(products);
                    }
                }
                else
                {
                    return StatusCode(401, new Response(ResponseType.Failed, "User Unauthorized"));
                }
            }
            catch(Exception e)
            {
                return StatusCode(500, new Response(ResponseType.ServerError, "Something went wrong, please contact server admin"));
            }
      
        }
    }
}
