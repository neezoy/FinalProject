using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {


        static async Task Send<T>(NetworkStream stream, T message)
        {


        }

        static async Task Receive<T>(NetworkStream stream)
        {

        }

        static (byte[] header, byte[] body) Encode<T>(T message)
        {

        }


        static void Main(string[] args)
        {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();


            IPAddress connectionIP = IPAddress.Loopback; //change this for deployment

            IPEndPoint endpoint = new IPEndPoint(connectionIP, 9999);
            Socket socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);


            NetworkStream stream = new NetworkStream(socket, true);

            String stringMessage = "Test";
            
            //send
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(stringMessage);
            stream.Write(buffer, 0, buffer.Length); // buffer, offset, length
            Console.WriteLine("Received: " + stringMessage);

            //receive
            byte[] response = new byte[1024];
            stream.Read(response, 0, response.Length);
            String stringResponse = System.Text.Encoding.UTF8.GetString(response);

            //display response
            Console.WriteLine("Received: " + stringResponse);
            Console.ReadLine();

        }
    }
}
