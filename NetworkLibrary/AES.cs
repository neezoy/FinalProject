using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetworkLibrary
{
    public class AES
    {
        private RijndaelManaged rijndael;
        
        public AES(byte[] keyData, byte[] IV)
        {
            rijndael = new RijndaelManaged();
            rijndael.Key = keyData;
            rijndael.IV = IV;
        }
        public AES()
        {
            rijndael = new RijndaelManaged();
            rijndael.GenerateKey();
            rijndael.GenerateIV();
        }
        public String AESDecrypt(String encryptedStr)
        {
            byte[] encrypted = Convert.FromBase64String(encryptedStr);

            ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

            String decryptedMessage;

            // Crypto steam. Using for Idisposible 
            using (MemoryStream msDecrypt = new MemoryStream(encrypted))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        //Write to stream
                        decryptedMessage = srDecrypt.ReadToEnd();
                    }
                }
            }

            return decryptedMessage;
        }

        public String AESEncrypt(String message)
        {

            ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
            byte[] encrypted;

            // Crypto steam. Using for Idisposible 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write to stream
                        swEncrypt.Write(message);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            //Covert Byte Array to String
            String encryptedStr = Convert.ToBase64String(encrypted);
            return encryptedStr;
        }

            
        

        public (byte[] key, byte[] IV) GetKeyAndIV()
        {
            return (rijndael.Key, rijndael.IV);
        }
    }
}
