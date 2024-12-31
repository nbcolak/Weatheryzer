using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Weatheryzer.Function.Services;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Function
{
    public class WeatherTimerTrigger(
        HttpClient httpClient,
        IBlobStorageService blobStorageService,
        IQueueService queueService,
        IWeatherService weatherService)
    {
        [Function("WeatherTimerTrigger")]
        public async Task Run([TimerTrigger("1 * * * * *")]
            TimerInfo timerInfo, 
            FunctionContext context)
        {
            var logger = context.GetLogger("WeatherTimerTrigger");
            logger.LogInformation($"Timer function executed at: {DateTime.Now}");

            try
            {
                //get weather data
                var weatherData = await weatherService.GetWeatherAsync();
                weatherData.EnsureSuccessStatusCode(); 
                var responseData = await weatherData.Content.ReadAsStringAsync();
                logger.LogInformation("Weather data api called successfully.");
                // Blob Storage
                var blobName = await blobStorageService.SaveWeatherDataToBlobAsync(responseData);
                logger.LogInformation("Weather data saved to Blob Storage successfully.");
                //azure queue
                await queueService.SendMessageAsync(blobName);
                logger.LogInformation("Weather data processed and sent to queue successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }
        }
    }
}