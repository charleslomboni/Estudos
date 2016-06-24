using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using RabbitMQ.Client;

namespace RabbitMQTest
{
    class Program
    {
        static volatile bool _cancelling;

        static void Main(string[] args)
        {
            Console.Title = "Producer";
            Thread.Sleep(1000);

            #region [ exemplo 1 ]
            ////var connectionFactory = new ConnectionFactory();
            ////cria a conexão usando as configurações de GetRabbitMqConnection
            //IConnection connection = new RabbitMqService().GetRabbitMqConnection();
            ////Cria o canal para habilitar multiplas threads de comunicação
            //IModel channel = connection.CreateModel();
            ////Cria a fila nomeada "Hello-world-queue"
            //channel.QueueDeclare("Hello-world-queue", false, false, false, null);
            ////Array de bytes de hello world
            //byte[] message = Encoding.UTF8.GetBytes("Hello, World!");
            //channel.BasicPublish(string.Empty, "Hello-world-queue", null, message);

            //Console.WriteLine("Pressione qualquer tecla para sair..");
            //Console.ReadKey();
            //channel.Close();
            //connection.Close();
            #endregion [ exemplo 1 ]

            #region [ exemplo 2 ]
            //var connectionFactory = new ConnectionFactory();
            //IConnection connection = connectionFactory.CreateConnection();
            //IModel channel = connection.CreateModel();
            //channel.QueueDeclare("hello-world-queue", false, false, false, null);
            //byte[] message = Encoding.UTF8.GetBytes("Hello, World!");
            //channel.BasicPublish(string.Empty, "hello-world-queue", null, message);
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
            //string value = DoSomethingInteresting();
            //string logMessage = string.Format("{0}: {1}", TraceEventType.Information, value);

            //byte[] message = Encoding.UTF8.GetBytes(logMessage);
            //channel.BasicPublish("direct-exchange-example", "", null, message);

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();
            //channel.Close();
            //connection.Close();
            #endregion [ "exemplo direct-exchange-example" ]

            #region [ exemplo "fanout-exchange-example"]
            //var connectionFactory = new ConnectionFactory();
            //IConnection connection = connectionFactory.CreateConnection();
            //IModel channel = connection.CreateModel();

            //channel.ExchangeDeclare("fanout-exchange-example", ExchangeType.Fanout, false, true, null);
            //var thread = new Thread(() => PublishQuotes(channel));
            //thread.Start();

            //Console.WriteLine("Press 'x' to exit");
            //var input = (char)Console.Read();
            //_cancelling = true;

            //channel.Close();
            //connection.Close();
            #endregion [ exemplo "fanout-exchange-example" ]

            #region [ exemplo "topic-exchange-example"]
            //TopicExchanges.MainMethod();
            #endregion [ exemplo "topic-exchange-example" ]

            #region [ exemplo "header-exchange-example"]
            HeaderExchange.MainMethod();
            #endregion [ exemplo "header-exchange-example" ]
        }


        static string DoSomethingInteresting()
        {
            return Guid.NewGuid().ToString();
        }

        static void PublishQuotes(IModel channel)
        {
            while (true)
            {
                if (_cancelling) return;
                IEnumerable quotes = FetchStockQuotes(new[] { "GOOG", "HD", "MCD" });
                foreach (string quote in quotes)
                {
                    byte[] message = Encoding.UTF8.GetBytes(quote);
                    channel.BasicPublish("fanout-exchange-example", "", null, message);
                }
                Thread.Sleep(5000);
            }
        }

        static IEnumerable<string> FetchStockQuotes(string[] symbols)
        {
            var quotes = new List<string>();

            string url = string.Format("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})&env=store://datatables.org/alltableswithkeys",
                String.Join("%2C", symbols.Select(s => "%22" + s + "%22")));
            var wc = new WebClient { Proxy = WebRequest.DefaultWebProxy };
            var ms = new MemoryStream(wc.DownloadData(url));
            var reader = new XmlTextReader(ms);
            XDocument doc = XDocument.Load(reader);
            XElement results = doc.Root.Element("results");

            foreach (string symbol in symbols)
            {
                XElement q = results.Elements("quote").First(w => w.Attribute("symbol").Value == symbol);
                quotes.Add(symbol + ":" + q.Element("Ask").Value);
            }

            return quotes;
        }
    }
}
