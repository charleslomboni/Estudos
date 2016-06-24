using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
   public static class HeaderExchangeConsumerExtension
    {
        public static void StartConsume(this IModel channel, string queueName, Action<IModel, DefaultBasicConsumer, BasicDeliverEventArgs> callback)
        {
            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);

            while (true)
            {
                try
                {
                    var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    new Thread(() => callback(channel, consumer, eventArgs)).Start();
                }
                catch (EndOfStreamException)
                {
                    // The consumer was cancelled, the model closed, or the connection went away.
                    break;
                }
            }
        }
    }
}
