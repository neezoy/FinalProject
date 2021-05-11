using System;
using System.Threading;

namespace Server
{
    class Bootstrap
    {
        static void Main(string[] args)
        {



            //var cancel = new CancellationTokenSource();


            EchoServer echo = new EchoServer();

            echo.StartEcho(9999);
            Console.WriteLine("Echo Server Started!");
            Console.ReadLine();
        }
    }
}
