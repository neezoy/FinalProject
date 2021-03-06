//Inspired by Richard Weeks' Socket programming Tutorial https://github.com/zeul72/SocketsProgramming
using NetworkLibrary;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace WPFServer
{
    class Server
    {


        public void StartServer(int port, Action<string> consoleCallback)
        {

            consoleCallback("Setting Up Server...");
          

            IPAddress ipAddress = IPAddress.Loopback;
            var endPoint = new IPEndPoint(ipAddress, port);

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128); //Pending connections (backlog)

            consoleCallback("Server bound to: "+ ipAddress.ToString() + " Port: " + port);

            _ = Task.Run(() => ServerInit(socket, consoleCallback));
            //new Thread(ServerInit(socket, consoleCallback)).Start();
        }


        private async void ServerInit(Socket socket, Action<string> consoleCallback)
        {
            while (true)
            {
                
                    var socketWrappedInTask = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object, IAsyncResult>(socket.BeginAccept), //Begin Accept returns IAsyncResult
                    new Func<IAsyncResult, Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);

                
                consoleCallback("Echo Server: Client Connected!");

                using var stream = new NetworkStream(socketWrappedInTask, true);
                var buffer = new byte[1024];

                var prot = new Protocol();
                MessageHandler mh = new MessageHandler();



                while (true)
                {
                    var receivedMessage = await prot.Receive<MessageModel>(stream).ConfigureAwait(false);

                    

                    _= Task.Run(() => mh.Handle(receivedMessage,consoleCallback));
                    //Application.Current.Dispatcher.Invoke(new Action(() => { consoleCallback(a); }));


                    if (receivedMessage.MessageHeaderType != 11 && receivedMessage.MessageHeaderType != 12)
                    {
                        await prot.Send(stream, receivedMessage).ConfigureAwait(false);
                    }

                }

            }
        }
    }

}