using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace SkillNet.Notifications
{
    public class NegotiateSignalr
    {
        [Function("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "notificationHub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}
