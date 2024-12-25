using Weatheryzer.Application;
using Weatheryzer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddApplicationServices(); // Application Katmanındaki servisler
builder.Services.AddInfrastructureServices(builder.Configuration); // Infrastructure Katmanındaki servisler
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();