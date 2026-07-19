using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using MockService.Data;
using Serilog;
using System.Diagnostics;

namespace MockService;

public class Program
{
    public static void Main(string[] args)
    {
        ConfigureLogging();
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        try
        {
            Log.Information("Starting up");
            BuildAndRunWebApplication(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void BuildAndRunWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddSingleton<ISensorDataRepository, SensorDataRepository>();

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MockService", Version = "v1" });
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MockService v1"));

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/live", new()
        {
            Predicate = check => check.Tags.Contains("live")
        });

        app.Run();
    }

    private static void ConfigureLogging()
    {
        Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [s:{SessionId}][m:{MeetId}][p:{SpeakerId}] {Message:lj}{NewLine}{Exception}")
            .MinimumLevel.Debug()
            .CreateLogger();
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Error("UnhandledException: {unhandledExc} isTerminating:{isTerminating}", e.ExceptionObject, e.IsTerminating);
        Log.CloseAndFlush();
    }
}
