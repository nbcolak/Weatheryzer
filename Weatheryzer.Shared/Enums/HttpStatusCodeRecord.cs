namespace Weatheryzer.Shared.Enums;

public record HttpStatusCodeRecord(int Code, string Description)
{
    public static readonly HttpStatusCodeRecord Ok = new(200, "Ok");
    public static readonly HttpStatusCodeRecord Created = new(201, "Created");
    public static readonly HttpStatusCodeRecord NoContent = new(204, "No Content");
    public static readonly HttpStatusCodeRecord BadRequest = new(400, "Bad Request");
    public static readonly HttpStatusCodeRecord Unauthorized = new(401, "Unauthorized");
    public static readonly HttpStatusCodeRecord Forbidden = new(403, "Forbidden");
    public static readonly HttpStatusCodeRecord NotFound = new(404, "Not Found");
    public static readonly HttpStatusCodeRecord InternalServerError = new(500, "Internal Server Error");
}