using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Akka
using Akka;
using Akka.Actor;

namespace Test.Akka {

    // create an (immutable) message type that your actor will respond to
    public class Greet {
        public string Who { get; set; }

        public Greet(string who) {
            Who = who;
        }
    }

    // Create the actor class
    public class GreetingActor : ReceiveActor {

        public GreetingActor() {

            // Tell the actor to respond to the Greet messa
            Receive<Greet>(g => Console.WriteLine("Hello {0}", g.Who));
        }

    }

    class Program {
        static void Main(string[] args) {

            // Create a new actor system (a container for your actors)
            var system = ActorSystem.Create("MySyste");

            // Create your actor and get a reference to it.
            // This will be an "ActorRef", which is not a
            // reference to the actual actor instance
            // but rather a client or proxy to it.
            var greeter = system.ActorOf<GreetingActor>("greeter");

            // Send a message to the actor
            greeter.Tell(new Greet("World"));

            // This prevents the app from existing
            // before the async work is done
            Console.ReadLine();

        }
    }
}
