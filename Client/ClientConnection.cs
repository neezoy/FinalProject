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

    public class MyMessage
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }

    class ClientConnection
    {

        



        static async Task Start(string[] args)
        {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();


            IPAddress connectionIP = IPAddress.Loopback; //change this for deployment

            IPEndPoint endpoint = new IPEndPoint(connectionIP, 9999);
            Socket socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);


            NetworkStream stream = new NetworkStream(socket, true);

            var myMessage = new MyMessage
            {
                IntProperty = 404,
                StringProperty = "Hello World"
            };


            Console.WriteLine("Sending");
            Print(myMessage);

            var prot = new Protocol();

            await prot.Send(stream, myMessage).ConfigureAwait(false);

            var responseMsg = await prot.Receive<MyMessage>(stream).ConfigureAwait(false);

            Console.WriteLine("Received");
            Print(responseMsg);


            Console.ReadLine();


        }
        static void Print(MyMessage m) => Console.WriteLine($"MyMessage.IntProperty = {m.IntProperty}, MyMessage.StringProperty = {m.StringProperty}");
    }
}
