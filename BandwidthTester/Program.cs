using BandwidthTester;
using Serilog;

try
{
    Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.Seq("http://seq:5341")
                            .CreateLogger();

    IHost host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices(services =>
                    {
                        services.AddHostedService<Worker>();
                    })
                    .UseSerilog()
                    .Build();

    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The worker service crashed!");
}
finally{
    Log.CloseAndFlush();
}
