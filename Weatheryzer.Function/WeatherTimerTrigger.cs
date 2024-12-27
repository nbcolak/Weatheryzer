
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Weatheryzer.Function.Services;

namespace Weatheryzer.Function
{
    public class WeatherTimerTrigger(
        HttpClient httpClient,
        BlobServiceClient blobServiceClient,
        IWeatherService weatherService)
    {
        private readonly HttpClient _httpClient = httpClient;


        [Function("WeatherTimerTrigger")]
        public async Task Run([TimerTrigger("* * * * * *")] TimerInfo timerInfo, FunctionContext context)
        {
            var logger = context.GetLogger("WeatherTimerTrigger");
            logger.LogInformation($"Timer function executed at: {DateTime.Now}");

            try
            {
                // API
                var response = await weatherService.GetWeatherAsync();
                response.EnsureSuccessStatusCode(); 
                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Weather Data: {responseData}");
                
                // Blob Storage
                var blobContainer = blobServiceClient.GetBlobContainerClient("weatherdata");
                await blobContainer.CreateIfNotExistsAsync();

                var blobClient = blobContainer.GetBlobClient($"weather-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.json");
                using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseData));
                await blobClient.UploadAsync(stream, overwrite: true);

                logger.LogInformation("Weather data saved to Blob Storage successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }
        }
    }
}