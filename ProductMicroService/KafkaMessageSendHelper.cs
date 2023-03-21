using KafkaNet;
using KafkaNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductMicroService
{
    public class KafkaMessageSendHelper
    {
        public void KafkaMessagePublishConsume(string message)
        {
            //Integrate the Kafka Messaging
            Uri uri = new Uri("http://localhost:9092");
            string topic = "chat-message";
            string payload = message;
            var sendMessage = new Thread(() => {
                KafkaNet.Protocol.Message msg = new KafkaNet.Protocol.Message(payload);
                var options12 = new KafkaOptions(uri);
                var router = new BrokerRouter(options12);
                var client = new Producer(router);
                client.SendMessageAsync(topic, new List<KafkaNet.Protocol.Message> { msg }).Wait();
            });
            sendMessage.Start();


            string topicName = "chat-message";
            var options = new KafkaOptions(uri);
            var brokerRouter = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(topicName, brokerRouter));
            foreach (var msg in consumer.Consume())
            {
                Console.WriteLine(Encoding.UTF8.GetString(msg.Value));
            }
            Console.ReadLine();
        }
    }
}
