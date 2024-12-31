namespace Weatheryzer.Function.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string> GetBlobContentAsync(string blobName);

    Task<string> SaveWeatherDataToBlobAsync(string data);
}