using NetworkLibrary;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client
{


    public class ClientConnection
    {
        NetworkStream stream;

        public void Init()
        {
            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            IPAddress connectionIP = IPAddress.Loopback; //change this for deployment
            
            IPEndPoint endpoint = new IPEndPoint(connectionIP, 9999);
            Socket socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);

            stream = new NetworkStream(socket, true);
        }

        public async Task Send()
        {

            var myMessage = new MessageModel
            {
                MessageHeaderType = 0,
                MessageData = "Hello World"
            };

            Console.WriteLine("Sending");
            Print(myMessage);

            var prot = new Protocol();

            await prot.Send(stream, myMessage).ConfigureAwait(false);

            var responseMsg = await prot.Receive<MessageModel>(stream).ConfigureAwait(false);

            Console.WriteLine("Received");
            Print(responseMsg);
        }


        //public async Task Start()
        //{

        //    Console.WriteLine("Press Enter to Connect");
        //    Console.ReadLine();


        //    IPAddress connectionIP = IPAddress.Loopback; //change this for deployment

        //    IPEndPoint endpoint = new IPEndPoint(connectionIP, 9999);
        //    Socket socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //    socket.Connect(endpoint);


        //    NetworkStream stream = new NetworkStream(socket, true);

        //    var myMessage = new MessageModel
        //    {
        //        MessageHeaderType = 0,
        //        MessageData = "Hello World" 
        //    };


        //    Console.WriteLine("Sending");
        //    Print(myMessage);

        //    var prot = new Protocol();

        //    await prot.Send(stream, myMessage).ConfigureAwait(false);

        //    var responseMsg = await prot.Receive<MessageModel>(stream).ConfigureAwait(false);

        //    Console.WriteLine("Received");
        //    Print(responseMsg);


        //    Console.ReadLine();


        //}
        static void Print(MessageModel m) 
        { 
            Console.WriteLine($"Message.MessageHeaderType = {m.MessageHeaderType}, MyMessage.MessageData = {m.MessageData}");
        }
    }
}
