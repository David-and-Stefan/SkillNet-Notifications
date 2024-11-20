using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkillNet.Notifications.Entities;

namespace SkillNet.Notifications
{
    public class NotificationConsumerFunction
    {
        private readonly ILogger<NotificationConsumerFunction> _logger;
        private readonly IMongoCollection<Notification> _notificationsCollection;

        public NotificationConsumerFunction(ILogger<NotificationConsumerFunction> logger, IMongoCollection<Notification> notificationsCollection)
        {
            _logger = logger;
            _notificationsCollection = notificationsCollection;
        }

        [Function(nameof(NotificationConsumerFunction))]
        [SignalROutput(HubName = "notificationHub")]
        public async Task<SignalRMessageAction> Run(
            [ServiceBusTrigger("notification", "Test-Sub", Connection = "ServiceBusConnection")] string message)
        {
            _logger.LogInformation("Received message: {message}", message);

            var jsonObject = JObject.Parse(message);

            var messagePart = jsonObject["message"];
            if (messagePart == null)
            {
                _logger.LogWarning("No 'message' field found in the incoming JSON.");
                return null;
            }

            var notification = messagePart.ToObject<Notification>();

            await _notificationsCollection.InsertOneAsync(notification);

            return new SignalRMessageAction("receiveNotification")
            {
                Arguments = new[] { message }   
            };
        }
    }
}
