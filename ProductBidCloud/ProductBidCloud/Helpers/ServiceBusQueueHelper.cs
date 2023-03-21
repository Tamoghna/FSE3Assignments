using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductBidCloud.Helpers
{
    class ServiceBusQueueHelper
    {
        public void SendMessageToQueue(string message)
        {
            
            //Service bus Queue
            string connectionString = "Endpoint=sb://tamoghnafse3survicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kPfpO2Aoyy9yOlfJbDWtv7pKM5yaMQqQu+ASbOnoWZw=";
            string queueName = GetSecret("ServiceBusQueueName");


            //string queueName = "tamoghnafse3queue";
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            var client = new ServiceBusClient(connectionString);

            // create the sender
            ServiceBusSender sender = client.CreateSender(queueName);

            // create a message that we can send
            ServiceBusMessage objmessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));

            // send the message
            sender.SendMessageAsync(objmessage);
        }

        public string GetSecret(string secretName)
        {
            KeyVaultSecret keyValueSecret = null;
            try

            {
                SecretClient _secretClient = new SecretClient(new Uri("https://tamoghnafse3keyvault.vault.azure.net/"), new DefaultAzureCredential());
                keyValueSecret = _secretClient.GetSecret(secretName);
  
            }
            catch
            {
                throw;

            }
            return keyValueSecret.Value;

        }

    }
}
