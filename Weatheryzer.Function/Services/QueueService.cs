using System.Text.Json;
using Azure.Storage.Queues;
using MediatR;
using Weatheryzer.Application.WeatherForecast.Command;
using Weatheryzer.Application.WeatherForecast.Dtos;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Function.Services;

public class QueueService : IQueueService
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IMediator _mediator;
    private readonly QueueClient _queueClient;

    public QueueService(IMediator mediator, IBlobStorageService blobStorageService)
    {
        var connectionString = Environment.GetEnvironmentVariable("MyAzureStorageQueue");
        _queueClient = new QueueClient(connectionString, "weather-queue");
        _mediator = mediator;
        _blobStorageService = blobStorageService;
    }

    public async Task SendMessageAsync(string message)
    {
        await _queueClient.CreateIfNotExistsAsync();
        if (await _queueClient.ExistsAsync()) await _queueClient.SendMessageAsync(message);
    }

    public async Task ConsumeMessagesAsync()
    {
        await _queueClient.CreateIfNotExistsAsync();
        if (await _queueClient.ExistsAsync())
        {
            // Mesajları sırayla al
            var messages = await _queueClient.ReceiveMessagesAsync(10);

            if (messages.Value.Length > 0)
                foreach (var message in messages.Value)
                    try
                    {
                        var blobName = message.Body.ToString();
                        Console.WriteLine($"Received blob name: {blobName}");
                        await ProcessMessageAsync(blobName);
                        await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                        Console.WriteLine("Message processed and deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda logla
                        Console.WriteLine($"An error occurred while processing message: {ex.Message}");
                    }
        }
    }

    private async Task ProcessMessageAsync(string blobName)
    {
        var blobJson = await _blobStorageService.GetBlobContentAsync(blobName);
        var weatherData = JsonSerializer.Deserialize<Root>(blobJson);

        var command = new CreateWeatherCommand(
            weatherData?.location.name ?? "Unknown",
            weatherData?.current.temp_c ?? 0.0,
            weatherData?.current.condition.text ?? "Unknown",
            blobJson,
            weatherData?.current.humidity,
            AdditionalInfo: "",
            WindSpeed: weatherData?.current.wind_degree
        );

        await _mediator.Send(command);
    }
}