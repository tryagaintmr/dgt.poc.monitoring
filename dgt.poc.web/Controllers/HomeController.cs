using dgt.poc.web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;


namespace dgt.poc.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpGet("Weather")]
        public async Task<IActionResult> Weather()
        {
            try
            {
                _logger.LogInformation("Getting weather forecast...");

                // Replace with the actual URL of your API that provides the weather forecast
                var apiUrl = "http://host.docker.internal:8090/WeatherForecast";

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
                    return View(weatherData);
                }

                return View("Error", "Could not retrieve weather forecast");
            } catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return View();
        }
    }
}