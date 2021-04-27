using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace NetworkLibrary
{
    public class XMLEProtocol : Protocol
    {

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

    }
}
