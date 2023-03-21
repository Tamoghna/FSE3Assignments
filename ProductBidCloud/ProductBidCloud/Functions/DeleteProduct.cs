using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using ProductBidCloud.Models;
using System.Linq;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Bson;
using System.Net;
using System.Reflection;
using ProductBidCloud.Helpers;
using Azure.Messaging.ServiceBus;

namespace ProductBidCloud.Functions
{
    class DeleteProduct
    {
        private readonly MongoClient _mongoClient;
        private readonly IConfiguration _config;

        private readonly IMongoCollection<Product> _products;

        //Added for Application Insights 
        private readonly ILogger<DeleteProduct> _deleteProductlogger; // Adding the ILogger service  
     
        public DeleteProduct(
            MongoClient mongoClient,
            IConfiguration config,
            ILogger<DeleteProduct> logger)
        {
            _mongoClient = mongoClient;
            _deleteProductlogger = logger;
            _config = config;

           // var database = _mongoClient.GetDatabase(_config[Settings.DATABASE_NAME]);
           // _products = database.GetCollection<Product>(_config[Settings.COLLECTION_NAME]);

            var database = _mongoClient.GetDatabase("ProductsFSETamoghna");
            _products = database.GetCollection<Product>("TamoghnaFSE3Azure1");
        }

        [FunctionName("Delete_Product")]
        public IActionResult DeleteTodo(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "V1/Seller/Delete/{id}")] HttpRequest req,
    TraceWriter log, string id)
        {

            //Added for Application Insight Logging
            var iteration = 1;
            _deleteProductlogger.LogDebug($"Debug {iteration}");
            _deleteProductlogger.LogInformation($"Information {iteration}");
            _deleteProductlogger.LogWarning($"Warning {iteration}");
            _deleteProductlogger.LogError($"Error {iteration}");
            _deleteProductlogger.LogCritical($"Critical {iteration}");

            ServiceBusQueueHelper objServiceBusQueue = new ServiceBusQueueHelper();

            //objServiceBusQueue.SendMessageToQueue("Initial Test For KeyVault");

            try
            {
                if (id.Equals("2"))
                {
                    _deleteProductlogger.LogError("Category 2 items cannot be deleted");
                    //File.WriteAllText(@"C:\TEst.txt", "testhhhkh");
                }
                List<Product> items = new List<Product>();
                IMongoDatabase db = _mongoClient.GetDatabase(Constants.DATABASE_NAME);
                var collList = db.ListCollections().ToList();
                Console.WriteLine("The list of collections are :");

                foreach (var item in collList)
                {
                    Console.WriteLine(item);
                }
                var cars = db.GetCollection<BsonDocument>(Constants.COLLECTION_NAME);
                var filter = Builders<BsonDocument>.Filter.Eq("CategoryId", Convert.ToInt32(id));

                var builder = Builders<BsonDocument>.Filter;
                var filtergeneric = builder.Eq("CategoryId", Convert.ToInt32(id)) & builder.Gt("BidEndDate", DateTime.Now);

                int recordCount = cars.Find(filter).ToList().Count;
                int recordCountgeneric = cars.Find(filtergeneric).ToList().Count;
                if (recordCount > 1)
                {
                    if (recordCountgeneric == 0)
                    {
                        cars.DeleteMany(filter);
                    }
                    else
                    {
                        string exceptionReason = "Product Can not be deleted after the Bid End Date.";

                        
                        var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new System.Net.Http.StringContent(exceptionReason),
                            ReasonPhrase = exceptionReason
                        };

                        MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                        string methodName = method.Name;
                        string className = method.ReflectedType.Name;

                        string fullMethodName = className + "." + methodName;


                        _deleteProductlogger.LogError(exceptionReason + fullMethodName);
                        /*
                        Logger objLogger = new Logger();
                        objLogger.ExceptionFileLogging(exceptionReason, fullMethodName);

                        throw new System.Web.Http.HttpResponseException(response);
                        */
                    }
                
                }
                else
                {
                    string exceptionReason = "At Least One Bid Should be present for a specific product Category";
                    var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new System.Net.Http.StringContent(exceptionReason),
                        ReasonPhrase = exceptionReason
                    };

                    MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                    string methodName = method.Name;
                    string className = method.ReflectedType.Name;

                    string fullMethodName = className + "." + methodName;
                    Logger objLogger = new Logger();
                    objLogger.ExceptionFileLogging(exceptionReason, fullMethodName);

                    throw new System.Web.Http.HttpResponseException(response);
                }

                _deleteProductlogger.LogInformation("All the Products have been deleted with Category Id" + Convert.ToString(id));
               
                objServiceBusQueue.SendMessageToQueue("All the Products have been deleted with Category Id" + Convert.ToString(id));


            }
            catch (Exception ex)
            {
                _deleteProductlogger.LogError(ex, ex.Message);
            }
            /*
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            */

            return new OkResult();
        }
        /*
        [FunctionName(nameof(DeleteProduct))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/{categoryid}")] HttpRequest req,
            int categoryid)
        {
            IActionResult returnValue = null;

            try
            {
                var albumToDelete = _products.DeleteOne(album => album.CategoryId == categoryid);

                if (albumToDelete == null)
                {
                    _logger.LogInformation($"Product with CategoryId: {categoryid} does not exist. Delete failed");
                    returnValue = new StatusCodeResult(StatusCodes.Status404NotFound);
                }

                returnValue = new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not delete item. Exception thrown: {ex.Message}");
                returnValue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return returnValue;
        }
        */
    }
}

