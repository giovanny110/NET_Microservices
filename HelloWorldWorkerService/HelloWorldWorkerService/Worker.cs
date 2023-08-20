namespace HelloWorldWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Hello world! at: {time}", DateTimeOffset.Now);
                _logger.LogWarning("This is a warning");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}