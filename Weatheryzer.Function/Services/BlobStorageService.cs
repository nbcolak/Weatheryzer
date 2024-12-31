using Azure.Storage.Blobs;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Function.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> GetBlobContentAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("weatherdata");
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadContentAsync();
        return response.Value.Content.ToString();
    }

    public async Task<string> SaveWeatherDataToBlobAsync(string data)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("weatherdata");
        await blobContainer.CreateIfNotExistsAsync();

        var blobName = $"weather-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.json";
        var blobClient = blobContainer.GetBlobClient(blobName);
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobName;
    }
}