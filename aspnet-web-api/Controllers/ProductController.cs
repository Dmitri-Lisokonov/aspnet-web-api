using aspnet_web_api.Models;
using aspnet_web_api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace aspnet_web_api.Controllers
{
    [Route("/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductRepository _repo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _repo = new ProductRepository();
            _logger = logger;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            List<Product> products = _repo.GetAll();
            if (products.Count < 1)
            {
                return NotFound("No product found with that id");
            }
            if (products != null)
            {
                return Ok(products);
            }
            else
            {
                return StatusCode(500, "The server was unable to process your request due to server error or invalid paramaters");
            }
        }
    }
}
