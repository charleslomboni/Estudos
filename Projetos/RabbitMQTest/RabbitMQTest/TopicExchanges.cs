using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQTest
{
    public enum Sector
    {
        Personal,
        Business
    }

    interface ILogger
    {
        void Write(Sector sector, string entry, TraceEventType traceEventType);
    }

    class RabbitLogger : ILogger, IDisposable
    {
        readonly long _clientId;
        readonly IModel _channel;
        bool _disposed;

        public RabbitLogger(IConnection connection, long clientId)
        {
            _clientId = clientId;
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare("direct-exchange-example", ExchangeType.Topic, false, true, null);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_channel != null && _channel.IsOpen)
                {
                    _channel.Close();
                }
            }
            GC.SuppressFinalize(this);
        }

        public void Write(Sector sector, string entry, TraceEventType traceEventType)
        {
            byte[] message = Encoding.UTF8.GetBytes(entry);
            string routingKey = string.Format("{0}.{1}.{2}", _clientId, sector.ToString(), traceEventType.ToString());
            _channel.BasicPublish("topic-exchange-example", routingKey, null, message);
        }

        ~RabbitLogger()
        {
            Dispose();
        }
    }
    public class TopicExchanges
    {
        const long ClientId = 10843;
        public static void MainMethod()
        {
            var connectionFactory = new ConnectionFactory();
            IConnection connection = connectionFactory.CreateConnection();

            TimeSpan time = TimeSpan.FromSeconds(10);
            var stopwatch = new Stopwatch();
            Console.WriteLine("Running for {0} seconds", time.ToString("ss"));
            stopwatch.Start();

            while (stopwatch.Elapsed < time)
            {
                using (var logger = new RabbitLogger(connection, ClientId))
                {
                    Console.Write("Time to complete: {0} seconds\r", (time - stopwatch.Elapsed).ToString("ss"));
                    logger.Write(Sector.Personal, "This is an information message", TraceEventType.Information);
                    logger.Write(Sector.Business, "This is an warning message", TraceEventType.Warning);
                    logger.Write(Sector.Business, "This is an error message", TraceEventType.Error);
                    Thread.Sleep(1000);
                }
            }

            connection.Close();
            Console.Write("                             \r");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
