using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public static class Hook
    {
        public static void OnLoad()
        {
            Console.WriteLine("Hello From DLL");
        }


        public static void OnGameEvent(int a)
        {
            Console.WriteLine("Hello From DLL");
        }
    }
}
