using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService;

namespace Orders.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Greeter.GreeterClient _greeterClient;

        public OrdersController(IHttpClientFactory httpClientFactory, Greeter.GreeterClient greeterClient)
        {
            _httpClientFactory = httpClientFactory;
            _greeterClient = greeterClient;
        }

        [HttpGet]
        [Authorize]
        public async Task<string> Get()
        {
            var client = _httpClientFactory.CreateClient("client");
            try
            {
                var response = await client.GetStringAsync(new Uri("api/products", UriKind.Relative));
                return "Hello from secret orders " + response;
            }
            catch (Exception e)
            {
               return e.Message;
            }
        }
        
        [HttpGet]
        [Authorize]
        [Route("grpc")]
        public async Task<string> GetGrpc()
        {
           
            try
            {
                var result = await _greeterClient.SayHelloAsync(new HelloRequest
                {
                    Name = "Hello product from orders"
                });

                return result.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}