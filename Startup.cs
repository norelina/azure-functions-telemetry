using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
//using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(FunctionsTelemetry.Startup))]

namespace FunctionsTelemetry
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            // This is not needed for the TelemetryConfiguration
            //builder.Services.AddApplicationInsightsTelemetry();
        }
    }
}