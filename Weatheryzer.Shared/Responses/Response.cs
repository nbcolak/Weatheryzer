using Weatheryzer.Shared.Enums;

namespace Weatheryzer.Shared.Responses;

public class Response<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public HttpStatusCodeRecord StatusCode { get; set; }
    public string Message { get; set; }

    public Response(T data, bool success, HttpStatusCodeRecord statusCode, string message = null)
    {
        Data = data;
        Success = success;
        StatusCode = statusCode;
        Message = message;
    }
}