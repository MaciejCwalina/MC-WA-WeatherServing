using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace WeatherServing
{
    public class WeatherServingController
    {
        private readonly ILogger<WeatherServingController> _logger;

        public WeatherServingController(ILogger<WeatherServingController> logger)
        {
            this._logger = logger;
        }

        [Function("WeatherServingController")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
