using System;
using System.Reflection;

namespace DemoGameRPS
{
    class Program
    {
       

        public static void PlayRPS(Random ran, MethodInfo m, Object classInstance  )
        {

            int num = ran.Next(1, 11);
            String result = null;

            if (num <= 2) { result = "You Rolled Scissors!"; }        //20% 
            if (num >= 7) { result = "You Rolled Paper!"; }           //30%
            if (num < 7 && num > 2) { result = "You Rolled Rock!"; }  //50%
            Console.WriteLine(result);

            //hook Method 

            //load parameters
            object[] param = new object[1];
            param[0] = result;
            m.Invoke(classInstance, param);
            //send


        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To Rock Paper Scissors!");

            // hook start
            var dll = Assembly.LoadFrom(@"C:\Users\MacBook\Desktop\CSProject\FinalProject\Client\bin\Debug\netcoreapp3.1\Client.dll");
            Type classType = dll.GetType("Client.Hook");

            

            //check type
            if (classType == null){
                throw new Exception("DLL Type could not be loaded!");
            }
          
            //instanciate class
            var classInstance = Activator.CreateInstance(classType);

            //Load methods from class
            var onLoadmethod = classType.GetMethod("OnLoad");
            var onRollmethod = classType.GetMethod("OnRollEvent", new Type[] { typeof(String) });

            //check method
            if (onLoadmethod == null | onRollmethod == null)
            {
                throw new Exception("DLL Method could not be loaded!");
            }

            onLoadmethod.Invoke(classInstance, null);


            System.Random ran = new Random();

            while (true) { 
                PlayRPS(ran, onRollmethod, classInstance);
                Console.ReadLine();
            }
            

            
        }
    }
}
