using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SkillNet.Notifications
{
    public class NotificationConsumerFunction
    {
        private readonly ILogger<NotificationConsumerFunction> _logger;

        public NotificationConsumerFunction(ILogger<NotificationConsumerFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(NotificationConsumerFunction))]
        public void Run([ServiceBusTrigger("notification", "Test-Sub", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
