using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisConnectionTest
{
    //http://www.c-sharpcorner.com/UploadFile/2cc834/using-redis-cache-with-C-Sharp/
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            Console.WriteLine("Saving random data in cache");
            program.SaveBigData();

            Console.WriteLine("Reading data from cache");
            program.ReadData();

            Console.ReadLine();
        }

        public void ReadData()
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            var devicesCount = 10000;
            for (int i = 0; i < devicesCount; i++)
            {
                var value = cache.StringGet($"Device_Status:{i}");
                Console.WriteLine($"Valor={value}");
            }
        }


        public void SaveBigData()
        {
            var devicesCount = 10000;
            var rnd = new Random();
            var cache = RedisConnectorHelper.Connection.GetDatabase();

            for (int i = 1; i < devicesCount; i++)
            {
                var value = rnd.Next(0, 10000);
                cache.StringSet($"Device_Status:{i}", value);
            }
        }
    }
}
