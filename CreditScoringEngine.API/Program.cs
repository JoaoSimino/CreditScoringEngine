using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CreditScoringEngine.API.Endpoints;
using CreditScoringEngine.API.Middlewares;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.MemoryStorage;
using CreditScoringEngine.Infrastructure.Messaging;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Configura logger mínimo para permitir logs antes do banco existir
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Ambiente de testes usa InMemory
if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<CreditScoringEngineContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<CreditScoringEngineContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPropostaService, PropostaService>();
builder.Services.AddScoped<IPropostaProcessingService, PropostaProcessingService>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ClienteExceptionHandler>();
builder.Services.AddExceptionHandler<ClienteNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<PropostaExceptionHandler>();
builder.Services.AddExceptionHandler<PropostaNotFoundExceptionsHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});
builder.Services.AddHangfireServer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermissiveCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// --- CRIA BANCO SE NÃO EXISTIR ---
if (!builder.Environment.IsEnvironment("Test"))
{
    using (var scope = app.Services.CreateScope())
    {
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var fullConnectionString = config.GetConnectionString("DefaultConnection");

        var builderWithoutDb = new SqlConnectionStringBuilder(fullConnectionString)
        {
            InitialCatalog = "master"
        };

        var databaseName = new SqlConnectionStringBuilder(fullConnectionString).InitialCatalog;

        using var connection = new SqlConnection(builderWithoutDb.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = $@"
            IF DB_ID(N'{databaseName}') IS NULL
            BEGIN
                CREATE DATABASE [{databaseName}];
            END";

        command.ExecuteNonQuery();
    }

    // --- RECONFIGURA O SERILOG AGORA COM SINK NO BANCO, QUE JÁ ESTÁ CRIADO ---
    var outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
    var sinkOptions = new MSSqlServerSinkOptions
    {
        TableName = "Logs",
        AutoCreateSqlTable = true,
    };

    var columnOptions = new ColumnOptions();
    columnOptions.Store.Remove(StandardColumn.Properties);
    columnOptions.Store.Add(StandardColumn.LogEvent);
    columnOptions.TimeStamp.NonClusteredIndex = true;

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithProcessId()
        .Enrich.WithThreadId()
        .WriteTo.Console(outputTemplate: outputTemplate)
        .WriteTo.MSSqlServer(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
            sinkOptions: sinkOptions,
            columnOptions: columnOptions,
            restrictedToMinimumLevel: LogEventLevel.Information
        )
        .WriteTo.File(
            path: "Logs/app.log",
            outputTemplate: outputTemplate,
            rollingInterval: RollingInterval.Day,
            flushToDiskInterval: TimeSpan.FromSeconds(1),
            shared: true
        )
        .CreateLogger();
}

// Aplica migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CreditScoringEngineContext>();
    dbContext.Database.Migrate();
}

app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermissiveCors");
app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapClienteEndpoints();
app.MapPropostaEndpoints();

RecurringJob.AddOrUpdate<IPropostaProcessingService>(
    "Processar-Propostas-Pendentes",
    service => service.ProcessarPropostasPendentesAsync(),
    "*/2 * * * *");

app.Run();

public partial class Program { }
