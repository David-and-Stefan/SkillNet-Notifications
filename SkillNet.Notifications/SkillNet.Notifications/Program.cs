using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using SkillNet.Notifications.Entities;
using SkillNet.Notifications.Middlewares;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((worker) =>
    {
        worker.UseMiddleware<TokenValidationMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var connectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
        var mongoClient = new MongoClient(connectionString);
        services.AddSingleton<IMongoClient>(mongoClient);

        var database = mongoClient.GetDatabase("skillnet-notifications");
        services.AddSingleton(database);

        var notificationsCollection = database.GetCollection<Notification>("notifications"); 
        services.AddSingleton(notificationsCollection);
    })
    .Build();

host.Run();
