using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ScoriaDataProcessor
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, [ServiceBusTrigger("add-demo-data", Connection = "ScoriaDataConnection")] ServiceBusReceivedMessage message)
        {
            if(message.Body.ToString().Contains("Exception"))
            {
                throw new Exception();
            }
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
