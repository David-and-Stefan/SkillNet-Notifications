using SkillNet.Notifications.Attributes;

namespace SkillNet.Notifications
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using SkillNet.Notifications.Entities;
    using SkillNet.Notifications.Middlewares;
    using System.Net;

    public class DeleteNotificationFunction
    {
        private readonly ILogger<DeleteNotificationFunction> _logger;
        private readonly IMongoCollection<Notification> _notificationsCollection;

        public DeleteNotificationFunction(ILogger<DeleteNotificationFunction> logger, IMongoCollection<Notification> notificationsCollection)
        {
            _logger = logger;
            _notificationsCollection = notificationsCollection;
        }

        [Function("DeleteNotification")]
        [Authorize]

        public async Task<HttpResponseData> DeleteNotificationAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "notifications/{id}")] HttpRequestData req, FunctionContext context,
            string id)
        {
            if (!TokenValidationMiddleware.IsAuthenticated(context, out var unathenticatedResponse))
            {
                return unathenticatedResponse;
            }

            _logger.LogInformation($"Deleting notification with Id: {id}");

            

            var deleteResult = await _notificationsCollection.DeleteOneAsync(n => n.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Notification not found.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Notification with Id {id} deleted successfully.");
            return response;
        }
    }
}
