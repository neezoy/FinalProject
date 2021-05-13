using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkLibrary
{
    public class Protocol
    {
        public async Task Send<T>(NetworkStream stream, T message)
        {
            var (header, body) = Encode(message);
            await stream.WriteAsync(header, 0, header.Length).ConfigureAwait(false);
            await stream.WriteAsync(body, 0, body.Length);



        }

        public async Task<T> Receive<T>(NetworkStream stream)
        {
            var header = await ReadAsync(stream, 4); //only read 4 first bytes
            //Header bytes contain body length.
            var bodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header, 0)); //changing bytes from network order to Host order. 
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
            while (bytesRead < bytesToRead)
            {
               
                //Console.WriteLine("Buffeer: " +BitConverter.ToString(buffer));
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
    }
}
