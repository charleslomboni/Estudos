using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Consumer";

            #region [ exemplo 1 ]
            ////Conexão
            ////var connectionFactory = new ConnectionFactory();
            ////IConnection connection = connectionFactory.CreateConnection();
            ////IModel channel = connection.CreateModel();
            ////channel.QueueDeclare("hello-world-queue", false, false, false, null);

            ////cria a conexão usando as configurações de GetRabbitMqConnection
            //IConnection connection = new RabbitMqService().GetRabbitMqConnection();
            ////Cria o canal para habilitar multiplas threads de comunicação
            //IModel channel = connection.CreateModel();
            ////Cria a fila nomeada "Hello-world-queue"
            //channel.QueueDeclare("Hello-world-queue", false, false, false, null);

            ////Consumir a mensagem da fila
            //BasicGetResult result = channel.BasicGet("hello-world-queue", true);

            ////Validar se existe msg para imprimir na tela
            //if (result != null)
            //{
            //    string message = Encoding.UTF8.GetString(result.Body);
            //    Console.WriteLine(message);
            //}

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();
            //channel.Close();
            //connection.Close();
            #endregion [ exemplo 1 ]

            #region [ exemplo 2 ]
            //var connectionFactory = new ConnectionFactory();
            //IConnection connection = connectionFactory.CreateConnection();
            //IModel channel = connection.CreateModel();
            //channel.QueueDeclare("hello-world-queue", false, false, false, null);
            //BasicGetResult result = channel.BasicGet("hello-world-queue", true);
            //if (result != null)
            //{
            //    string message = Encoding.UTF8.GetString(result.Body);
            //    Console.WriteLine(message);
            //}
            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();
            //channel.Close();
            //connection.Close();
            #endregion [ exemplo 2 ]

            #region [ exemplo "direct-exchange-example" ]
            //var connectionFactory = new ConnectionFactory();
            //IConnection connection = connectionFactory.CreateConnection();
            //IModel channel = connection.CreateModel();

            //channel.ExchangeDeclare("direct-exchange-example", ExchangeType.Direct);
            //channel.QueueDeclare("logs", false, false, true, null);
            //channel.QueueBind("logs", "direct-exchange-example", "");

            //var consumer = new QueueingBasicConsumer(channel);
            //channel.BasicConsume("logs", true, consumer);
            //var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            //string message = Encoding.UTF8.GetString(eventArgs.Body);
            //Console.WriteLine(message);

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();
            //channel.Close();
            //connection.Close();
            #endregion [ exemplo "direct-exchange-example" ]

            #region [ exemplo "fanout-exchange-example" ]
            //var connectionFactory = new ConnectionFactory();
            //IConnection connection = connectionFactory.CreateConnection();
            //IModel channel = connection.CreateModel();

            //channel.ExchangeDeclare("fanout-exchange-example", ExchangeType.Fanout, false, true, null);
            //channel.QueueDeclare("quotes", false, false, true, null);
            //channel.QueueBind("quotes", "fanout-exchange-example", "");

            //var consumer = new QueueingBasicConsumer(channel);
            //channel.BasicConsume("quotes", true, consumer);

            //while (true)
            //{
            //    try
            //    {
            //        var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
            //        string message = Encoding.UTF8.GetString(eventArgs.Body);
            //        Console.WriteLine(message);
            //    }
            //    catch (EndOfStreamException)
            //    {
            //        // The consumer was cancelled, the model closed, or the connection went away.
            //        break;
            //    }
            //}

            //channel.Close();
            //connection.Close();
            #endregion [ exemplo "fanout-exchange-example" ]

            #region [ exemplo "topic-exchange-example"]
            //TopicExchangesConsumer.MainMethod();
            #endregion [ exemplo "topic-exchange-example" ]

            #region [ exemplo "header-exchange-example"]
            HeaderExchangeConsumer.MainMethod();
            #endregion [ exemplo "header-exchange-example" ]
        }
    }
}
