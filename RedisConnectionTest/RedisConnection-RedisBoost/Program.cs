using RedisBoost;
using System;
using System.Configuration;

namespace RedisConnection_RedisBoost {

    internal class Program {

        private static void Main(string[] args) {
            // RedisBoost usa connectionstring para configurar acesso ao Redis
            var connectionString = ConfigurationManager.ConnectionStrings["Redis"].ConnectionString;
            var client = RedisClient.ConnectAsync(connectionString).Result;

            SimpleExample(connectionString);
            ClassExample(connectionString);
            PubSubMessage(connectionString);
        }

        private static void SimpleExample(string connectionString) {
            // Conexão
            using (var pool = RedisClient.CreateClientsPool()) {
                IRedisClient redisClient;
                var redisKey = "RedisBoostKey";
                var redisKeyDelete = "RedisBoostKeyDelete";

                // Cria o client
                using (redisClient = pool.CreateClientAsync(connectionString).Result) {
                    // Faz de forma assincrona o inserir no Redis
                    redisClient.SetAsync(redisKey, "RedisBoost mandando bala!").Wait();

                    // Recupera de forma assincrona convertendo para string
                    // Outras formas de serializar com o RedisBoost
                    // https://github.com/andrew-bn/RedisBoost/wiki/Serialization
                    var redisReturn = redisClient.GetAsync(redisKey).Result.As<string>();
                    redisClient.ExpireAsync(redisKey, 60);
                }

                using (redisClient = pool.CreateClientAsync(connectionString).Result) {
                    redisClient.SetAsync(redisKeyDelete, "RedisBoost Delete key.").Wait();
                    var deleteKey = redisClient.GetAsync(redisKeyDelete).Result.As<string>();

                    // Deleta
                    var resultDelete = redisClient.DelAsync(redisKeyDelete).Result;
                }
            }
        }

        private static void ClassExample(string connectionString) {
            // Conexão
            using (var pool = RedisClient.CreateClientsPool()) {
                IRedisClient redisClient;
                var redisKey = "RedisBoostKeyClass";
                var redisKeyDelete = "RedisBoostKeyDeleteClass";

                var func = new Funcionario {
                    IdFuncionario = Guid.NewGuid(),
                    Name = "Charles",
                    LastName = "Lomboni",
                    Age = 28
                };

                // Cria o client
                using (redisClient = pool.CreateClientAsync(connectionString).Result) {
                    // Faz de forma assincrona o inserir no Redis
                    redisClient.SetAsync(redisKey, func).Wait();

                    // Recupera de forma assincrona convertendo para string
                    // Outras formas de serializar com o RedisBoost
                    // https://github.com/andrew-bn/RedisBoost/wiki/Serialization
                    var redisReturn = redisClient.GetAsync(redisKey).Result.As<Funcionario>();
                    redisClient.ExpireAsync(redisKey, 60);
                }

                using (redisClient = pool.CreateClientAsync(connectionString).Result) {
                    redisClient.SetAsync(redisKeyDelete, func).Wait();
                    var deleteKey = redisClient.GetAsync(redisKeyDelete).Result.As<Funcionario>();

                    // Deleta
                    var resultDelete = redisClient.DelAsync(redisKeyDelete).Result;
                }
            }
        }

        private static void PubSubMessage(string connectionString) {
            using (var pool = RedisClient.CreateClientsPool()) {
                IRedisClient redisClient;
                var redisKey = "RedisBoostKeyClass";
                var redisKeyDelete = "RedisBoostKeyDeleteClass";

                var func = new Funcionario {
                    IdFuncionario = Guid.NewGuid(),
                    Name = "Charles",
                    LastName = "Lomboni",
                    Age = 28
                };


                // Cria o client
                using (redisClient = pool.CreateClientAsync(connectionString).Result) {
                    
                    // Faz de forma assincrona o inserir no Redis
                    using (var subscriber = redisClient.SubscribeAsync("channel").Result) {

                        // Cria o client
                        using (var redisClient2 = pool.CreateClientAsync(connectionString).Result) {
                            using (var publisher = redisClient2) {
                                publisher.PublishAsync("channel", func).Wait();
                            }
                        }

                        var channelMessage = subscriber.ReadMessageAsync(ChannelMessageType.Message |
                                                                         ChannelMessageType.PMessage).Result;
                        var c = channelMessage.Channels[0];
                        var r = channelMessage.Value.As<Funcionario>();
                    }
                }
            }
        }
    }

    public class Funcionario {
        public Guid IdFuncionario { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}