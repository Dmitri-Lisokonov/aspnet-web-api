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
        public List<Product> getAll()
        {
            return _repo.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public List<Product> getAll(int id)
        {
            return _repo.GetById(id);
        }

    }
}
