using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.RollingFile;

namespace Exemplo_Serilog {
    class Program {
        static void Main(string[] args) {

            //// Uma ILogger é criado usando LoggerConfiguration
            //var log = new LoggerConfiguration().WriteTo.ColoredConsole().CreateLogger();

            //log.Information("Inicio do Serilog!");

            //Log.Logger = log;
            //Log.Information("O log global foi configurado");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile("logs\\myapp-{Date}.txt")
                .CreateLogger();

            Log.Information("Hello, world!");

            int a = 10, b = 0;
            try {
                Log.Debug("Dividing {A} by {B}", a, b);
                Console.WriteLine(a / b);
            } catch (Exception ex) {
                Log.Error(ex, "Something went wrong");
            }

            Log.CloseAndFlush();
            Console.ReadKey();
        }
    }
}
