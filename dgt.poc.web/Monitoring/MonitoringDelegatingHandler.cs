using Prometheus;
using System.Diagnostics;

namespace dgt.poc.web.Monitoring
{
    public class MonitoringDelegatingHandler : DelegatingHandler
    {
        private static readonly Counter TotalApiCalls = Metrics
            .CreateCounter("api_calls_total", "Total number of calls made to the API.");

        private static readonly Counter FailedApiCalls = Metrics
            .CreateCounter("api_calls_failed_total", "Total number of failed calls to the API.");

        private static readonly Histogram ApiCallDuration = Metrics
            .CreateHistogram("api_call_duration_seconds", "Histogram of API call durations in seconds.");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TotalApiCalls.Inc();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                stopwatch.Stop();
                ApiCallDuration.Observe(stopwatch.Elapsed.TotalSeconds);

                if (!response.IsSuccessStatusCode)
                {
                    FailedApiCalls.Inc();
                }

                return response;
            }
            catch
            {
                FailedApiCalls.Inc();
                throw;
            }
        }
    }

}
