using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Function;

public class WeatherQueueTimerTrigger(IQueueService queueService, ILogger<WeatherQueueTimerTrigger> logger)
{
    [Function("WeatherQueueTimerTrigger")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("WeatherQueueTimerTrigger executed at: {time}", DateTime.UtcNow);

        try
        {
            await queueService.ConsumeMessagesAsync();
            logger.LogInformation("Queue messages consumed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while consuming messages: {exception}", ex.Message);
        }
    }
}