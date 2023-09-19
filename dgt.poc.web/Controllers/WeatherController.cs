using dgt.poc.web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dgt.poc.web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public WeatherController(HttpClient httpClient, ILogger<HomeController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet("Weather")]
        public async Task<IActionResult> GetWeatherForecast()
        {
            // Replace with the actual URL of your API that provides the weather forecast
            var apiUrl = "Weather";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(content);
                var weatherData = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
                return View("WeatherForecast", weatherData);
            }

            return View("Error", "Could not retrieve weather forecast");
        }
    }
}
