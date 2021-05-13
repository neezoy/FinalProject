using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkLibrary
{
    public class ProtocolPlusAES: Protocol
    {


        public async Task Send<T>(NetworkStream stream, T message, byte[] key, byte[] IV)
        {


            var (header, body) = Encode(message,  key, IV);
            await stream.WriteAsync(header, 0, header.Length).ConfigureAwait(false);
            await stream.WriteAsync(body, 0, body.Length);
        }


        

        static (byte[] header, byte[] body) Encode<T>(T message, byte[] key, byte[] IV)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);

            //Serialize xml message to string with stringWriter and stores it in the stringbuilder.
            xml.Serialize(stringWriter, message);

            AES aes = new AES(key, IV);
            String encrypted = aes.AESEncrypt(stringBuilder.ToString());

            var bodyBytes = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());

            //calculates the length of body and creates header
            var headerBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bodyBytes.Length));

            return (headerBytes, bodyBytes);

        }

    }
}
