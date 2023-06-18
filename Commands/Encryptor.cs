using System.Security.Cryptography;
using System.Text;

namespace SingleEncrypter.Commands
{
    internal class Encryptor : Command
    {
        public override string? CommandName { get; set; }

        public override string? Option { get => base.Option; set => base.Option = value; }

        public override void ExecuteCommand(string[] args)
        {
            try
            {
                string path = Path.GetFullPath(args[1]);

                if (File.Exists(path))
                {
                    Encrypt(path, args[2]);
                }
                else
                {
                    Console.WriteLine($"""

                    File does not exist: {path}

                    """);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ENC needs valid path", ex);
            }
        }

        private static void Encrypt(string file, string key) 
        {         
            //TODO: block writing permition to the .enc file
            Aes aes = Aes.Create();

            byte[] fileBytes = File.ReadAllBytes(file);

            aes.Key = DeriveKey(key);
            aes.IV = new byte[16];
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] encFile = encryptor.TransformFinalBlock(fileBytes, 0, fileBytes.Length);

            File.WriteAllBytes(file + ".enc", encFile);
            File.Delete(file);
        }

        public static byte[] DeriveKey(string privateKey)
        {
            byte[] privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);
            byte[] hash = SHA256.HashData(privateKeyBytes);

            return hash;
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "enc" ? args[0] : "";
            return args[0] == "enc";
        }
    }
}
