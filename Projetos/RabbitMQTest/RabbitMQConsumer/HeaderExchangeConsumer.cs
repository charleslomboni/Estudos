using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public class HeaderExchangeConsumer
    {
        const string QueueName = "header-exchange-example";
        const string ExchangeName = "header-exchange-example";

        public static void MainMethod()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Headers, false, true, null);
            channel.QueueDeclare(QueueName, false, false, true, null);

            var specs = new Dictionary<string, object>();
            specs.Add("x-match", "any");
            specs.Add("key1", "12345");
            specs.Add("key2", "123455");
            channel.QueueBind(QueueName, ExchangeName, string.Empty, specs);

            channel.StartConsume(QueueName, MessageHandler);
            connection.Close();
        }

        public static void MessageHandler(IModel channel, DefaultBasicConsumer consumer, BasicDeliverEventArgs eventArgs)
        {
            string message = Encoding.UTF8.GetString(eventArgs.Body);
            Console.WriteLine("Message received: " + message);
            foreach (var headerKey in eventArgs.BasicProperties.Headers)
            {
                var convertedKey = Encoding.ASCII.GetString((byte[])headerKey.Value);
                Console.WriteLine(headerKey.Key + ": " + convertedKey);
            }

            if (message == "quit")
                channel.BasicCancel(consumer.ConsumerTag);
        }
    }
}
