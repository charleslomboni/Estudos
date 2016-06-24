using StackExchange.Redis;
using System;

namespace RedisConnectionTest {

    internal class RedisConnectorHelper {

        // Explicação e implementação do padrão Singleton
        // http://csharpindepth.com/articles/general/singleton.aspx

        // Configurando uma conexão segura para o Redis
        // http://gigi.nullneuron.net/gigilabs/setting-up-a-connection-with-stackexchange-redis/
        // ConfigurationOptions explicações
        // https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Configuration.md
        private static Lazy<ConfigurationOptions> configOptions = new Lazy<ConfigurationOptions>(() => {
            var configOptions = new ConfigurationOptions();
            configOptions.EndPoints.Add("localhost:6379");
            configOptions.ClientName = "SafeRedisConnection";
            configOptions.ConnectTimeout = 100000;
            configOptions.SyncTimeout = 100000;
            configOptions.AbortOnConnectFail = false;
            return configOptions;
        });

        // Lazy Connection
        // https://msdn.microsoft.com/en-us/library/dd997286(v=vs.110).aspx
        private static Lazy<ConnectionMultiplexer> conn = new Lazy<ConnectionMultiplexer>(() =>
        ConnectionMultiplexer.Connect(configOptions.Value));

        public static ConnectionMultiplexer SafeConn {
            get { return conn.Value; }
        }
    }
}