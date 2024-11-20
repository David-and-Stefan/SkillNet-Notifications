using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SkillNet.Notifications.Entities;
using System.Net;
using SkillNet.Notifications.Attributes;
using SkillNet.Notifications.Middlewares;

namespace SkillNet.Notifications
{
    public class GetNotificationsFunction
    {
        private readonly ILogger<GetNotificationsFunction> _logger;
        private readonly IMongoCollection<Notification> _notificationsCollection;

        public GetNotificationsFunction(ILogger<GetNotificationsFunction> logger, IMongoCollection<Notification> notificationsCollection)
        {
            _logger = logger;
            _notificationsCollection = notificationsCollection;
        }

        [Function("GetNotifications")]
        [Authorize]
        public async Task<HttpResponseData> GetNotificationsAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "notifications")] HttpRequestData req,
            FunctionContext context)
        {
            if (!TokenValidationMiddleware.IsAuthenticated(context, out var unathenticatedResponse))
            {
                return unathenticatedResponse;
            }

            _logger.LogInformation("Fetching notifications...");

            // Parse the optional userId parameter from query string
            var queryParameters = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var userId = queryParameters["userId"];

            // Build filter based on userId if provided
            var filter = string.IsNullOrEmpty(userId) ? Builders<Notification>.Filter.Empty : Builders<Notification>.Filter.Eq(n => n.UserId, userId);

            var notifications = await _notificationsCollection.Find(filter).ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(notifications);
            return response;
        }
    }
}
