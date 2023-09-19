using Prometheus;
using System.Diagnostics;

namespace dgt.poc.api.Business
{
    public class ExtractDataLongOperation : IExtractDataLongOperation
    {
        private static readonly Random Randomizer = new Random();

        private static readonly Counter TotalMethodCalls = Metrics
            .CreateCounter("myApi_mybusinessclass_extractdata_calls_total", "Total number of calls made to the ExtractDataLongOperation method.");

        private static readonly Counter FailedMethodCalls = Metrics
            .CreateCounter("myApi_mybusinessclass_extractdata_calls_failed_total", "Total number of failed calls to the ExtractDataLongOperation method.");

        private static readonly Histogram MethodCallDuration = Metrics
            .CreateHistogram("myApi_mybusinessclass_extractdata_call_duration_seconds", "Histogram of ExtractDataLongOperation method call durations in seconds.");

        public async Task<bool> ExtractData()
        {
            TotalMethodCalls.Inc();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Simulate random business logic duration between 1 and 20 seconds
                int randomDuration = Randomizer.Next(1, 21); // Generate a random number between 1 and 20
                await Task.Delay(TimeSpan.FromSeconds(randomDuration));

                stopwatch.Stop();
                MethodCallDuration.Observe(stopwatch.Elapsed.TotalSeconds);
                return true;
            }
            catch
            {
                FailedMethodCalls.Inc();
                throw;
            }
        }
    }
}
