using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DL.Services
{
    public class PasswordHasher
    {
        private static readonly string encryptionPassword = "Ent3r your oWn S@lt v@lu# h#r3 Ya Mo3alem";

        private static readonly byte[] salt = Encoding.ASCII.GetBytes("Ent3r your oWn S@lt v@lu# h#r3 Ya Mo3alem");

        public static string HashPassword(string textToEncrypt)
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
        
        public static bool IsEqual(string source, string hashed)
        {
            return  HashPassword(source) == hashed;
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