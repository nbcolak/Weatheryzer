namespace Weatheryzer.Shared.Responses;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}