using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWebShop.APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
#pragma warning disable S1075 // URIs should not be hardcoded
        private const string catalogueUrl = "http://localhost:8082/api/values";
#pragma warning restore S1075 // URIs should not be hardcoded

        public GatewayController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<JsonResult> Index()
        {
            var catalogueAPIClient = this._httpClientFactory.CreateClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, catalogueUrl);
            requestMessage.Headers.Add("Accept", "application/json");
            var responseMessage = await catalogueAPIClient.SendAsync(requestMessage);
            return new JsonResult(responseMessage.Content);
        }
    }
}