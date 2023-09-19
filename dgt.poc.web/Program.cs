using dgt.poc.web.Monitoring;
using Prometheus;
using Serilog;

namespace dgt.poc.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console() // Configure Serilog to write logs to the console
                    .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient("MyHttpClient", client =>
            {
                client.BaseAddress = new Uri("http://host.docker.internal:8090/");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Log.Information("certificate");
                    // Implement your custom validation logic here.
                    // Return true if the certificate is trusted, false otherwise.
                    return true; // Example: Trust all certificates (for development purposes)
                };
                return handler;
            }).AddHttpMessageHandler<MonitoringDelegatingHandler>();


            var app = builder.Build();
            app.UseHttpMetrics();
            app.UseMetricServer();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}