using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ProductBidCloud.Helpers;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ProductBidCloud.Helpers
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            Constants objcon = new Constants();
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter(level => true);
            });

            var config = (IConfiguration)builder.Services.First(d => d.ServiceType == typeof(IConfiguration)).ImplementationInstance;

            builder.Services.AddSingleton((s) =>
            {
                //MongoClient client = new MongoClient(config[Settings.MONGO_CONNECTION_STRING]);
                //MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
                //Code Added for Azure deployment
                string connectionString = Constants.MONGO_CONNECTION_STRING;
                MongoClientSettings settings = MongoClientSettings.FromUrl(
                  new MongoUrl(connectionString)
                );
                settings.SslSettings =
                  new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                var client = new MongoClient(settings);

                return client;
            });
            builder.Services.AddApplicationInsightsTelemetry();
            
        }
    }
}
