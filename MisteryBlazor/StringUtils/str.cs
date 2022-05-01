using System.Security.Cryptography;
using System.Text;

namespace MisteryBlazor.StringUtils
{
    public static class str
    {
        private const string V = "7355607";
        private const int Keysize = 256;
        private const string split = "p";
        private const int rd = 9268;

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static int GetIntId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int root = BitConverter.ToInt32(buffer, 0);
            Random random = new Random(root);
            return random.Next(0, Int32.MaxValue - 1);
        }

        public static string ToASCIIByte(this string str)
        {
            try
            {
                string input = str.ToBase64();
                StringBuilder sb = new();
                var byteArray = Encoding.ASCII.GetBytes(input);
                foreach (var value in byteArray)
                {
                    sb.Append(value + rd).Append(split);
                }
                return byteArray.Length > 0 ? sb.ToString() : input;
            }
            catch (Exception e)
            {
                return str;
            }
        }
        public static string ToStringFromASCIIByte(this string str)
        {
            try
            {
                var input = str.Split(split);
                List<byte> b = new();
                for (int i = 0; i <= input.Length - 2; i++)
                {
                    b.Add(Convert.ToByte(Convert.ToInt32(input[i]) - rd));
                }
                string result = Encoding.ASCII.GetString(b.ToArray()).ToStringFromBase64();
                return result;
            }
            catch (Exception e)
            {
                return str;
            }
        }
        public static string EncryptString(this string input)
        {
            string str = input.ToBase64();
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(str);
            using var password = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(V), saltStringBytes, 1000);
            var keyBytes = password.GetBytes(Keysize / 8);
            using var symmetricKey = new RijndaelManaged();
            symmetricKey.BlockSize = 128;
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;
            using var encryption = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryption, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = saltStringBytes;
            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecryptString(this string cipherText)
        {
            try
            {
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2)
                    .Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();
                using var password = new Rfc2898DeriveBytes(V, saltStringBytes, 1000);
                var keyBytes = password.GetBytes(Keysize / 8);
                using var symmetricKey = new RijndaelManaged();
                symmetricKey.BlockSize = 128;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);
                using var memoryStream = new MemoryStream(cipherTextBytes);
                using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                var plainTextBytes = new byte[cipherTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).ToStringFromBase64();
            }
            catch (Exception e)
            {
                return cipherText;
            }
        }

        public static string ToBase64(this string str)
        {
            string st = " ";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                st = Convert.ToBase64String(bytes);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                return str;
            }
        }

        public static string ToStringFromBase64(this string s)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(s);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception e)
            {
                return s;
            }
        }
    }
}
