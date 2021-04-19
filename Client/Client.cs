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

    class Client
    {


       


        static async Task Send<T>(NetworkStream stream, T message)
        {
            var (header, body) = Encode(message);
            await stream.WriteAsync(header, 0, header.Length).ConfigureAwait(false);
            await stream.WriteAsync(body, 0, body.Length);



        }

        static async Task<T> Receive<T>(NetworkStream stream)
        {
            var header = await ReadAsync(stream, 4); //only read 4 first bytes
            //Header bytes contain body length.
            var bodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header)); //changing bytes from network order to Host order. 

            var bodyBytes = await ReadAsync(stream, bodyLength);

            return Decode<T>(bodyBytes);
        }

        static (byte[] header, byte[] body) Encode<T>(T message)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);

            //Serialize xml message to string with stringWriter and stores it in the stringbuilder.
            xml.Serialize(stringWriter, message);


            var bodyBytes = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());

            //calculates the length of body and creates header
            var headerBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bodyBytes.Length));

            return (headerBytes, bodyBytes);

        }

        static T Decode<T>(byte[] body)
        {
            var s = System.Text.Encoding.UTF8.GetString(body);

            //creating reader for xml deserializer
            StringReader stringReader = new StringReader(s);

            XmlSerializer xml = new XmlSerializer(typeof(T));

            //deserialize from stringto XML
            return (T)xml.Deserialize(stringReader);


        }

        static async Task<byte[]> ReadAsync(NetworkStream stream, int bytesToRead)
        {
            var buffer = new byte[bytesToRead];
            var bytesRead = 0;
            while(bytesRead < bytesToRead)
            {
                //runs in seperate thread, so I wont return it untill all bytes have been read.
                var bytesReceived = bytesRead + await stream.ReadAsync(buffer, bytesRead, (bytesToRead - bytesRead)).ConfigureAwait(false);            
                
                if (bytesReceived == 0)
                {
                    throw new Exception("Socket Closed");
                }
                bytesRead = bytesRead + bytesReceived;
            }

            return buffer;
        }


        static async Task Main(string[] args)
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

            await Send(stream, myMessage).ConfigureAwait(false);

            var responseMsg = await Receive<MyMessage>(stream).ConfigureAwait(false);

            Console.WriteLine("Received");
            Print(responseMsg);


            Console.ReadLine();


        }
        static void Print(MyMessage m) => Console.WriteLine($"MyMessage.IntProperty = {m.IntProperty}, MyMessage.StringProperty = {m.StringProperty}");
    }
}
