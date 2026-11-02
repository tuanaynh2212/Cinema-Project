using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace cinema_project.Logging
{
    public static class AppLogging
    {
        private static ILoggerFactory _factory = LoggerFactory.Create(_ => { });

        public static void Initialize()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            Directory.CreateDirectory(dir);
            Console.SetOut(TextWriter.Synchronized(new StreamWriter(Path.Combine(dir, $"cinema-project-{DateTime.Now:yyyyMMdd}.log"), true) { AutoFlush = true }));
            _factory = LoggerFactory.Create(b => b
                .AddSimpleConsole(o =>
                {
                    o.ColorBehavior = LoggerColorBehavior.Disabled;
                    o.SingleLine = true;
                    o.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                })
                .SetMinimumLevel(LogLevel.Information));
        }

        public static ILogger<T> CreateLogger<T>() => _factory.CreateLogger<T>();
    }
}