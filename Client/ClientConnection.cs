using NetworkLibrary;
using System;
using System.IO;
using System.Linq;
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
        byte[] aesKey;
        byte[] aesIV;
        Boolean AESSetup = true;
        Boolean AESEnabled = true;
        int curSendHeaderCode = 0;

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

        public async void AESInit()
        {
            //Init AES
            AES aes = new AES();
            (this.aesKey, this.aesIV) = aes.GetKeyAndIV();


            var prot = new Protocol();

            var keyMessage = new MessageModel
            {
                MessageHeaderType = 11,
                TimeData = DateTime.Now.ToString("HH:mm:ss tt"),
                MessageData = Convert.ToBase64String(aesKey)
            };


            //String a = Convert.ToBase64String(aesKey);
            //var equal = Convert.FromBase64String(a).SequenceEqual(aesKey);
            //Console.WriteLine(equal);

            Console.WriteLine("Sending AES KEY: " + Convert.ToBase64String(aesKey));
            await prot.Send(stream, keyMessage);
            //System.Threading.Thread.Sleep(1000);
            //MessageModel responseMsgKey = await prot.Receive<MessageModel>(stream);
            //Print(responseMsgKey);
            

            var IVMessage = new MessageModel
            {
                MessageHeaderType = 12,
                TimeData = DateTime.Now.ToString("HH:mm:ss tt"),
                MessageData = Convert.ToBase64String(aesIV)
            };
            Console.WriteLine("Sending IV KEY: " + Convert.ToBase64String(aesIV));
            await prot.Send(stream, IVMessage);
            //MessageModel responseMsgIV = await prot.Receive<MessageModel>(stream);
            //Print(responseMsgIV);

           
            AESSetup = false ;
        }

        public async Task Send(String result)
        {

            if (AESEnabled == true)
            {
                curSendHeaderCode = 10;
                
                AES aes = new AES(aesKey, aesIV);

                result = aes.AESEncrypt(result);
            }



            var myMessage = new MessageModel
            {
                MessageHeaderType = curSendHeaderCode,
                TimeData = DateTime.Now.ToString("HH:mm:ss tt"),
                MessageData = result
            };

            Console.WriteLine("Sending");
            Print(myMessage);

            var prot = new Protocol();

            await prot.Send(stream, myMessage).ConfigureAwait(false);

            var responseMsg = await prot.Receive<MessageModel>(stream).ConfigureAwait(false);

            Console.WriteLine("Received");
            Print(responseMsg);
        }


       
        static void Print(MessageModel m) 
        {
            
            Console.WriteLine($"Message.MessageHeaderType = {m.MessageHeaderType}, TimeData = {m.TimeData}, MyMessage.MessageData = {m.MessageData}");
        }
    }
}
