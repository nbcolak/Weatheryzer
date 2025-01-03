using Serilog;
using Weatheryzer.Application;
using Weatheryzer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; 

// Dependency Injection
builder.Services.AddApplicationServices(); // Application Katmanındaki servisler
builder.Services.AddInfrastructureServices(builder.Configuration); // Infrastructure Katmanındaki servisler
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() 
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(configuration["Serilog:SeqUrl"]) 
    .CreateLogger();

/*Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq(configuration["Serilog:SeqUrl"])
    .CreateLogger();*/

builder.Logging.ClearProviders(); 
builder.Logging.AddSerilog();   
builder.Host.UseSerilog();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();