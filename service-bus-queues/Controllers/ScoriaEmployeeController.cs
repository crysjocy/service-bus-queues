using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

namespace service_bus_queues.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoriaEmployeeController : ControllerBase
    {
        private readonly ServiceBusClient _client;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ScoriaEmployeeController> _logger;

        public ScoriaEmployeeController(ServiceBusClient client, ILogger<ScoriaEmployeeController> logger)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task Post(WeatherForecast data) 
        {
            var sender = _client.CreateSender("add-demo-data");
            var body = JsonSerializer.Serialize(data);
            var message = new ServiceBusMessage(body);
            if (body.Contains("scheduled")) message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddSeconds(15);
            await sender.SendMessageAsync(message);
        }
    }
}
