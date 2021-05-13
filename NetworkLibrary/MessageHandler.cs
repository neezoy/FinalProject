using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    public class MessageHandler
    {
        byte[] AESKey = null;
        byte[] AESIV = null;

        public async Task Handle(MessageModel m)
        {
            var message = m;
            

            switch (m.MessageHeaderType)
            {
                case 0:
                    Console.WriteLine("Message Type: Plain Text Message");
                    break;
                case 10:
                    Console.WriteLine("\nMessage Type: AES encrypted!");
                    Console.WriteLine("Encrypted: " + m.MessageData);
                    AESDecrypt(m.MessageData);
                    break;
                case 11:
                    Console.WriteLine("Message Type: AES Key = " + m.MessageData);
                    AESKey = Convert.FromBase64String(m.MessageData);
                    break;
                case 12:
                    Console.WriteLine("Message Type: AES IV = " + m.MessageData);
                    AESIV = Convert.FromBase64String(m.MessageData);
                    break;
                default:
                    throw new Exception("Message Header Type not recognized!");
                    

            }


            void AESDecrypt(String msg)
            {
                AES aes = new AES(AESKey, AESIV);
                Console.WriteLine("Decryped: " + aes.AESDecrypt(msg));
            }

        }
    }
}
