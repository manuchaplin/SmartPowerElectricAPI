using System.Security.Cryptography;
using System.Text;

namespace SmartPowerElectricAPI.Utilities
{
    public class EncryptHelper
    {
        public static string GetSHA1(string str)
        {
            if (String.IsNullOrEmpty(str)) return String.Empty;

            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        private static string encriptKey = "%dSd43$3dg&DEf/1";

        public static string EncryptData(string textToEncrypt)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textToEncrypt);
            byte[] keyBytes = Encoding.UTF8.GetBytes(encriptKey);
            byte[] ivBytes = new byte[16];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(ivBytes);
            }

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (var cipherStream = new System.IO.MemoryStream())
                {

                    using (var cryptoStream = new CryptoStream(cipherStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    byte[] cipherBytes = cipherStream.ToArray();
                    byte[] resultBytes = new byte[cipherBytes.Length + ivBytes.Length];

                    Buffer.BlockCopy(ivBytes, 0, resultBytes, 0, ivBytes.Length);
                    Buffer.BlockCopy(cipherBytes, 0, resultBytes, ivBytes.Length, cipherBytes.Length);

                    return Convert.ToBase64String(resultBytes);
                }
            }
        }

        public static string DecryptData(string textToDecrypt)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(textToDecrypt);

                byte[] ivBytes = new byte[16];
                byte[] encryptedBytes = new byte[cipherBytes.Length - 16];

                Buffer.BlockCopy(cipherBytes, 0, ivBytes, 0, ivBytes.Length);
                Buffer.BlockCopy(cipherBytes, ivBytes.Length, encryptedBytes, 0, encryptedBytes.Length);

                byte[] keyBytes = Encoding.UTF8.GetBytes(encriptKey);

                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;

                    using (var plainTextStream = new System.IO.MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(plainTextStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        byte[] plaintextBytes = plainTextStream.ToArray();

                        return Encoding.UTF8.GetString(plaintextBytes, 0, plaintextBytes.Length);
                    }
                }

            }
            catch
            {
                return "ERROR";
            }

        }
    }
}
