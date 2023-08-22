using SpeedTest;
using SpeedTest.Models;

namespace BandwidthTester;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly SpeedTestClient _client;
    private readonly Settings _settings;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _client = new SpeedTestClient();
        _settings = _client.GetSettings();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var servers = GetTestServers();
            double averageDownloadSpeed = Math.Round(GetAverageDownloadSpeed(servers), 0) / 1000;
            double averageUploadSpeed = Math.Round(GetAverageUploadSpeed(servers), 0) / 1000;

            _logger.LogInformation("{LogMessage} at {time}= Download: {download}, Upload: {upload} MBit/s"
                , "BandWidthTest"
                , DateTimeOffset.Now
                , averageDownloadSpeed
                , averageUploadSpeed);

            await Task.Delay(60000, stoppingToken);
        }
    }

    #region Private
    private IEnumerable<Server> GetTestServers()
    {
        _logger.LogInformation("Get Test Servers");

        var servers = _settings.Servers.OrderBy(server => server.Distance).Take(10);
        foreach(var server in servers){
            server.Latency = _client.TestServerLatency(server);
        }
        
        return servers.OrderBy(server => server.Latency).Take(2);
    }

    private double GetAverageDownloadSpeed(IEnumerable<Server> servers)
    {
        _logger.LogInformation("Testing Download Speed");

        var downloadSpeeds = new List<double>();
        foreach(var server in servers){
            double speed = _client.TestDownloadSpeed(server, _settings.Download.ThreadsPerUrl);
            downloadSpeeds.Add(speed);
        }

        return downloadSpeeds.Average();
    }

    private double GetAverageUploadSpeed(IEnumerable<Server> servers)
    {
        _logger.LogInformation("Testing Upload Speed");

        var uploadSpeeds = new List<double>();
        foreach(var server in servers)
        {
            double speed = _client.TestUploadSpeed(server, _settings.Upload.ThreadsPerUrl);
            uploadSpeeds.Add(speed);
        }

        return uploadSpeeds.Average();
    }
    #endregion
}
