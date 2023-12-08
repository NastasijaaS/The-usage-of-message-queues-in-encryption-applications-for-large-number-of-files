using System;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;


namespace DiplomskiRad
{
    class AES
    {

        private static byte[] key;
        private static byte[] iv;

        public void Initialize(byte[] userKey, byte[] userIV)
        {
            key = userKey;
            iv = userIV;
        }
        public  async Task<byte[]> EncryptFileAES(byte[] plainText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            await csEncrypt.WriteAsync(plainText, 0, plainText.Length);

                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
            catch(CryptographicException ex)
            {
                throw new Exception("An exception occurred during AES encryption: ", ex);
            }
        }

        public async Task<byte[]> DecryptFileAES(byte[] cipherText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                        {
                            await cs.WriteAsync(cipherText, 0, cipherText.Length);
                        }

                        return ms.ToArray();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                throw new Exception("An exception occurred during AES encryption: ", ex);
            }
        }

        public byte[] GenerateRandomKey()
        {
            byte[] key = GenerateRandomBytes(32);
            return key;
        }

        public byte[] GenerateRandomIV()
        {
            byte[] IV=GenerateRandomBytes(16);
            return IV;
      
        }

        public byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public string BytesToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
