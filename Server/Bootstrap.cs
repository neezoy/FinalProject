using System;

namespace Server
{
    class Bootstrap
    {
        static void Main(string[] args)
        {
            
            EchoServer echo = new EchoServer();

            echo.StartEcho(9999);
            Console.WriteLine("Echo Server Started!");
            Console.ReadLine();
        }
    }
}
