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
        [SignalROutput(HubName = "notificationHub")]
        public SignalRMessageAction Run(
            [ServiceBusTrigger("notification", "Test-Sub", Connection = "ServiceBusConnection")] string message)
        {
            _logger.LogInformation("Received message: {message}", message);

            return new SignalRMessageAction("receiveNotification")
            {
                Arguments = new[] { message }   
            };
        }
    }
}
