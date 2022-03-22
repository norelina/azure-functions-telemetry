using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
// using Microsoft.ApplicationInsights;
// using Microsoft.ApplicationInsights.Extensibility;

namespace FunctionsTelemetry
{
    public class TestTelemetry
    {
        // private readonly TelemetryClient telemetryClient;
        //
        // public TestTelemetry(TelemetryConfiguration telemetryConfiguration)
        // {
        //     this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        // }
        
        [FunctionName("test-telemetry")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("LOGGER: C# HTTP trigger function processed a request.");
            //telemetryClient.TrackTrace("TELEMETRYCLIENT: C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name ??= data?.name;

            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("BLOB_STORAGE_CONNECTION_STRING"));
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("telemetry");
            var blobClient = blobContainerClient.GetBlobClient($"test-{DateTime.Now}");
            await blobClient.UploadAsync(new BinaryData("test"), overwrite: true);

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
