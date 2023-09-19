using dgt.poc.api.Business;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics;

namespace dgt.poc.monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly Counter TotalMethodCalls = Metrics
            .CreateCounter("myApi_mybusinessclass_extractdata_calls_total", "Total number of calls made to the ExtractDataLongOperation method.");

        private static readonly Counter FailedMethodCalls = Metrics
            .CreateCounter("myApi_mybusinessclass_extractdata_calls_failed_total", "Total number of failed calls to the ExtractDataLongOperation method.");

        private static readonly Histogram MethodCallDuration = Metrics
            .CreateHistogram("myApi_mybusinessclass_extractdata_call_duration_seconds", "Histogram of ExtractDataLongOperation method call durations in seconds.");

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IExtractDataLongOperation _businessOperation;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IExtractDataLongOperation extractDataLongOperation)
        {
            _logger = logger;
            _businessOperation = extractDataLongOperation;
            Metrics.CreateCounter("myApi_db_create_requests_total", "Total number of create requests to the database.");
            Metrics.CreateHistogram("myApi_db_read_duration_seconds", "Histogram of read request durations in seconds.");
            Metrics.CreateGauge("myApi_db_active_connections", "Number of active database connections.");

        }

        [HttpGet(Name = "GetForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Increment the TotalMethodCalls counter
            TotalMethodCalls.Inc();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Simulate some work here...
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
            catch
            {
                // Increment the FailedMethodCalls counter in case of an error
                FailedMethodCalls.Inc();
                throw;
            }
            finally
            {
                stopwatch.Stop();

                // Observe the duration in seconds using the histogram
                MethodCallDuration.Observe(stopwatch.Elapsed.TotalSeconds);
            }

            
            
        }

        [HttpGet("extract-data")]
        public async Task<IActionResult> ExtractData()
        {
            // Increment the TotalMethodCalls counter
            TotalMethodCalls.Inc();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Simulate some work here...
                return Ok(await _businessOperation.ExtractData());
            }
            catch
            {
                // Increment the FailedMethodCalls counter in case of an error
                FailedMethodCalls.Inc();
                throw;
            }
            finally
            {
                stopwatch.Stop();

                // Observe the duration in seconds using the histogram
                MethodCallDuration.Observe(stopwatch.Elapsed.TotalSeconds);
            }

            
        }
    }
}