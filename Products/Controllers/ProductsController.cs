using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Products.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        public ProductsController(ILogger<ProductsController> logger)
        {
        }

        [HttpGet]
        [Authorize]
        public string Get()
        {
            return "Hello from secret product";
        }
    }
}