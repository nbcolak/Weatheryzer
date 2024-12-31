namespace Weatheryzer.Function.Services.Interfaces;

public interface IQueueService
{
    Task SendMessageAsync(string message);
    Task ConsumeMessagesAsync();
}