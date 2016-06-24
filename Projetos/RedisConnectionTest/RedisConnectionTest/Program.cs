using System;
using StackExchange.Redis;

namespace RedisConnectionTest {

    //http://www.c-sharpcorner.com/UploadFile/2cc834/using-redis-cache-with-C-Sharp/
    internal class Program {

        private static void Main(string[] args) {
            var program = new Program();

            program.ExampleSaveData();

            //program.ExampleSubPub();

            //Console.WriteLine("Saving random data in cache");
            //program.SaveBigData();

            //Console.WriteLine("Reading data from cache");
            //program.ReadData();

            Console.ReadLine();
        }

        public void ReadData() {
            var cache = RedisConnectorHelper.SafeConn.GetDatabase();
            var devicesCount = 10000;
            for (int i = 0; i < devicesCount; i++) {
                var value = cache.StringGet($"Device_Status:{i}");
                Console.WriteLine($"Valor={value}");
            }
        }

        public void SaveBigData() {
            var devicesCount = 10000;
            var rnd = new Random();
            var cache = RedisConnectorHelper.SafeConn.GetDatabase();

            for (int i = 1; i < devicesCount; i++) {
                var value = rnd.Next(0, 10000);
                cache.StringSet($"Device_Status:{i}", value);
            }
        }

        public void ExampleSaveData() {

            // Acessando a base de dados
            IDatabase db = RedisConnectorHelper.SafeConn.GetDatabase();

            var func = new Funcionario {
                IdFuncionario = Guid.NewGuid(),
                Name = "Charles",
                LastName = "Lomboni",
                Age = 28
            };

            var redisKey = "funcionariokey";

            // Data Types aceitos no Redis
            // http://redis.io/topics/data-types
            db.StringSet(redisKey, "ID: " + func.IdFuncionario + " - Nome: " + func.Name + " " + func.LastName + ", Idade: " + func.Age);

            // Força 60 segundos para a chave expirar
            db.KeyExpire(redisKey, TimeSpan.FromSeconds(60));

            // Pega o tempo faltando para a chave expirar
            var timeToLive = db.KeyTimeToLive(redisKey);

            // Pega a string contina na chave
            var retornoRedis = db.StringGet(redisKey);

            // Cria uma chave para deletar como exemplo
            db.StringSet("redisKeyToDelete", "Serei deletada!");
            var wasDeleted = db.KeyDelete("redisKeyToDelete");

        }

        public void ExampleSubPub() {
            // Obtem uma pub/sub subscriber connection
            ISubscriber sub = RedisConnectorHelper.SafeConn.GetSubscriber();

            // Prepara para o canal "messages" para receber mensagens.
            sub.Subscribe("messages", (channel, message) => {
                Console.WriteLine(message.ToString());
            });

            // Publica a mensagem "Hello world!" no canal "messages"
            sub.Publish("messages", "Hello world!");

        }
    }

    public class Funcionario {
        public Guid IdFuncionario { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}