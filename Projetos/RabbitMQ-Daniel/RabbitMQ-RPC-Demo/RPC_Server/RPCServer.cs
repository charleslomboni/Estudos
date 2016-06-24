using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPC_Server {
    class RPCServer {
        public static void Main() {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //criando a conexao
            using (var connection = factory.CreateConnection())
            //criando o canal e declarando a queue
            using (var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: "rpc_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                //o segundo parametro usado no BaicQos, serve para criarmos quantos server process quisermos.
                channel.BasicQos(0, 2, false);
                //criando um basic consumer para acessar a queue
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                                     noAck: false,
                                     consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                //usado para ficar pegando os requests e criando os responses
                while (true) {
                    string response = null;

                    //pegando o request
                    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                    //pegando as informações do request
                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try {
                        var message = Encoding.UTF8.GetString(body);
                        int n = int.Parse(message);
                        Console.WriteLine(" [.] fib({0})", message);

                        //preenchendo o response
                        response = fib(n).ToString();
                    } catch (Exception e) {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    } finally {
                        //publicando o response na fila de resposta para o cliente
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "",
                                             routingKey: props.ReplyTo,
                                             basicProperties: replyProps,
                                             body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                         multiple: false);
                    }
                }
            }
        }

        /// <summary>
        /// Assumes only valid positive integer input.
        /// Don't expect this one to work for big numbers,
        /// and it's probably the slowest recursive implementation possible.
        /// </summary>
        private static int fib(int n) {
            if (n == 0 || n == 1) {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }
    }
}
