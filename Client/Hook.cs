using System;
using System.Collections.Generic;
using System.Text;
using NetworkLibrary;


namespace Client
{
    public class Hook
    {
        ClientConnection connection;
        
        public void OnLoad()
        {
            Console.WriteLine("Game client has been hooked!");

            connection = new ClientConnection();
            connection.Init();
        }


        public async void OnRollEvent(int a)
        {
            Console.WriteLine("Hello From Roll: " +  a);

            await connection.Send().ConfigureAwait(false);
        }
    }
}
