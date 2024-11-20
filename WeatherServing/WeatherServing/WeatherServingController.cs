using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace WeatherServing
{
    public class WeatherServingController
    {
        private readonly ILogger<WeatherServingController> _logger;
        private readonly HttpClient _httpClient;  
        public WeatherServingController(ILogger<WeatherServingController> logger, IHttpClientFactory httpClientFactory)
        {
            this._logger = logger;
            this._httpClient = httpClientFactory.CreateClient();
        }

        [Function("GetWeather")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            req.HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*"); // Update with actual origin in production
            req.HttpContext.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            req.HttpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");

            String? lat = req.Query["lat"];
            String? lon = req.Query["lon"];

            if (String.IsNullOrEmpty(lat) || String.IsNullOrEmpty(lon)) {
                return new BadRequestObjectResult("Parameters cannot be empty");
            }

            HttpResponseMessage resp = await this._httpClient.GetAsync($"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&hourly=temperature_2m&forecast_days=1");
            if (!resp.IsSuccessStatusCode)
            {
                return new BadRequestObjectResult($"Failed to get the weather info response status code is: {resp.StatusCode}\n" +
                    $"The servers response is {resp.ReasonPhrase}");
            }

            return new OkObjectResult(await resp.Content.ReadAsStringAsync());
        }
    }
}
