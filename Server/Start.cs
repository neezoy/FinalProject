using System;
using System.Threading;

namespace Server
{
    class Start
    {
        static void Main(string[] args)
        {



            //var cancel = new CancellationTokenSource();


            ConsoleEchoServer echo = new ConsoleEchoServer();

            echo.StartEcho(9999);
            Console.WriteLine("Echo Server Started!");
            Console.ReadLine();
        }
    }
}
