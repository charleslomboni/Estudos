using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Log.Consumer {

    internal class Consumer {

        public static void Main() {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //criando a conexao
            using (var connection = factory.CreateConnection())
            //criandoo  canal
            using (var channel = connection.CreateModel()) {
                //declarando o exchange e o exchangeType, onde fanout funciona fazendo broadcast das mensagens para todas as filas
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                //criando uma fila temporaria
                var queueName = channel.QueueDeclare().QueueName;
                //fazendo boun da fiila ao exchange
                channel.QueueBind(queue: queueName,
                                  exchange: "logs",
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}