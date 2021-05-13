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
            connection.AESInit();
        }


        public async void OnRollEvent(String result)
        {
            Console.WriteLine("Hello From DLL! Sending Roll!");

            await connection.Send(result).ConfigureAwait(false);
        }
    }
}
