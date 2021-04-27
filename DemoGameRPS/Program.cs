using System;
using System.Reflection;

namespace DemoGameRPS
{
    class Program
    {


        public static void PlayRPS(Random ran )
        {

            int num = ran.Next(1, 11);

            if (num <= 2) { Console.WriteLine("Scissors!"); }        //20% 
            if (num >= 7) { Console.WriteLine("Paper!"); }           //30%
            if (num < 7 && num > 2) { Console.WriteLine("Rock!"); }  //50%

            //hook Method 
            //send
            

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To Rock Paper Scissors!");

            // hook program
            var dll = Assembly.LoadFile(@"C:\Users\MacBook\Desktop\CSProject\FinalProject\Client\bin\Debug\netcoreapp3.1\Client.dll");
            Type type = dll.GetType("Client.Hook");
            if(type != null)
            {
                var onLoadmethod = type.GetMethod("OnLoad");

                

                if (onLoadmethod != null)
                {
                    onLoadmethod.Invoke(null,null);
                }
                else
                {
                    Console.WriteLine("method is null");
                }
            }
            else
            {
                Console.WriteLine("Null type class");
            }

            
           
               
            
            System.Random ran = new Random();

            while (true) { 
                PlayRPS(ran);
                Console.ReadLine();
            }
            

            
        }
    }
}
