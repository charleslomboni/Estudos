using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPC_Client {
    class RPCClient {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public RPCClient() {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            //quando chamamos o QueueDeclare sem parametros, estamos criando uma fila nao duravel, exclusiva, auto apagavel
            //quando desconectarmos o consumidor da fila, a fila vai ser apagada automaticamente
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName,
                                 noAck: true,
                                 consumer: consumer);
        }

        public string Call(string message) {
            //passa uma chave no qujrest
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", //enviando para o exchange default
                                 routingKey: "rpc_queue",
                                 basicProperties: props,
                                 body: messageBytes);

            while (true) {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                //verifica se a chave do response, é igual a chave passada no request
                if (ea.BasicProperties.CorrelationId == corrId) {
                    return Encoding.UTF8.GetString(ea.Body);
                }
            }
        }

        public void Close() {
            connection.Close();
        }
    }

    class RPC {
        public static void Main() {

            while (true) {
                var rpcClient = new RPCClient();

                var number = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine(" [x] Requesting fib({0})", number);
                var response = rpcClient.Call(number.ToString());
                Console.WriteLine(" [.] Got '{0}'", response);

                rpcClient.Close();

               
            }
        }
    }
}
