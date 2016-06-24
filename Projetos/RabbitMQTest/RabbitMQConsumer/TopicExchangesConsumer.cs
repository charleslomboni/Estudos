using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
   public class TopicExchangesConsumer
    {
        public static void MainMethod()
        {
            var connectionFactory = new ConnectionFactory();
            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.ExchangeDeclare("topic-exchange-example", ExchangeType.Topic, false, true, null);
            channel.QueueDeclare("log", false, false, true, null);
            channel.QueueBind("log", "topic-exchange-example", "*.Personal.*");

            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume("log", true, consumer);

            while (true)
            {
                try
                {
                    var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    string message = Encoding.UTF8.GetString(eventArgs.Body);
                    Console.WriteLine(string.Format("{0} - {1}", eventArgs.RoutingKey, message));
                }
                catch (EndOfStreamException)
                {
                    // The consumer was cancelled, the model closed, or the connection went away.
                    break;
                }
            }

            channel.Close();
            connection.Close();
        }
    }
}
