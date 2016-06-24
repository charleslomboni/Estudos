using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQTest
{
    public class RabbitMqService
    {
        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";

            return connectionFactory.CreateConnection();
        }

        private static void SetupInitialTopicQueue(IModel model)
        {
            model.QueueDeclare("queueFromVisualStudio", true, false, false, null);
            model.ExchangeDeclare("exchangeFromVisualStudio", ExchangeType.Topic);
            model.QueueBind("queueFromVisualStudio", "exchangeFromVisualStudio", "superstars");
        }
    }
}
