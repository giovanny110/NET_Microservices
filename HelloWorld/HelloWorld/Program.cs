using Serilog;

Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();

LogData firstLog = new LogData() { Message = "Hello World", Number = 123 };

Log.Information("{Message}, {Number}", firstLog.Message, firstLog.Number);
Log.Information("{@log}", firstLog);


class LogData
{
    public string Message { get; set; }
    public int Number { get; set; }
}