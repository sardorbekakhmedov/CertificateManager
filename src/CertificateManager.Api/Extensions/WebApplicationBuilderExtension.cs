using Serilog;
using Serilog.Events;

namespace CertificateManager.Api.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddSerilogConfiguration(this WebApplicationBuilder builder)
    {
        var exceptionsPath = @"Logs\Exceptions.txt";
        var informationPath = @"Logs\Informations.txt";
        var maxLogFileCount = 100;

        CleanUpOldLogs(exceptionsPath, maxLogFileCount);
        CleanUpOldLogs(informationPath, maxLogFileCount);

        var logger = new LoggerConfiguration()
            .WriteTo.File(exceptionsPath, LogEventLevel.Error, rollingInterval: RollingInterval.Day)
            .WriteTo.File(informationPath, LogEventLevel.Information, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Logging.AddSerilog(logger);
    }

    private static void CleanUpOldLogs(string logFilePath, int maxLogFileCount)
    {
        var directory = Path.GetDirectoryName(logFilePath);
        var fileName = Path.GetFileName(logFilePath);

        if (string.IsNullOrWhiteSpace(directory)) return;

        var logFiles = Directory.GetFiles(directory, fileName + "*")
            .OrderBy(f => new FileInfo(f).LastWriteTime)
            .ToList();

        if (logFiles.Count <= maxLogFileCount) return;

        try
        {
            File.Delete(logFiles.First());
        }
        catch (IOException ex)
        {
            Log.Error(ex, "Error when deleting old log file: {fileName}", logFiles.First());
        }
    }

}