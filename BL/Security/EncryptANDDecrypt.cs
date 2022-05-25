using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BL.Security
{
    public static class EncryptANDDecrypt
    {
        private static readonly string encryptionPassword = "Ent3r your oWn S@lt v@lu# h#r3 Ya Mo3alem";
       

       

        private static readonly byte[] salt = Encoding.ASCII.GetBytes("Ent3r your oWn S@lt v@lu# h#r3 Ya Mo3alem");

        public static string EncryptText(string textToEncrypt)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }
            return Convert.ToBase64String(encryptedBytes);
        }
        public static string DecryptText(string encryptedText)
        {
            try
            {
                var algorithm = GetAlgorithm(encryptionPassword);

                byte[] descryptedBytes;
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
                }
                return Encoding.UTF8.GetString(descryptedBytes);
            }
            catch(Exception ex)
            {
                return "#";
            }
        }

        private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            try
            {
                MemoryStream memory = new MemoryStream();
                using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
                {
                    stream.Write(data, 0, data.Length);
                }
                return memory.ToArray();
            }
            catch { return new byte[] { }; }
        }
        private static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

            // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }
    }
}