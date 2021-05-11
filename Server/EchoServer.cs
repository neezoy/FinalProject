﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class EchoServer
    {

        public void StartEcho(int port)
        {
            Console.WriteLine("Setting Up Server...");

            IPAddress ipAddress = IPAddress.Loopback;
            var endPoint = new IPEndPoint(ipAddress, port);
        
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128); //Pending connections (backlog)

            Console.WriteLine($"Server bound to: {ipAddress.ToString()}, Port: {port}");

            _ = Task.Run(() => EchoInit(socket));

        }

        private async Task EchoInit(Socket socket)
        {
            while(true)
            {
                var socketWrappedInTask = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object, IAsyncResult>(socket.BeginAccept), //Begin Accept returns IAsyncResult
                    new Func<IAsyncResult,Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);

                Console.WriteLine("Echo Server: Client Connected!");

                using var stream = new NetworkStream(socketWrappedInTask, true);
                var buffer = new byte[1024];


                while(true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);


                    //if no bytes, socket is gone
                    if(bytesRead == 0)
                    {
                        break;
                    }

                    await stream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);

                } 


            } 
        }

    }
}
