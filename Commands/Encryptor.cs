using System.Security.Cryptography;
using System.Text;
using SingleEncrypter.Helper;

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
                    ---------------
                    - File does not exist 
                    - Path provided: {path}
                    ---------------
                    """);
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"""
                    ---------------
                    - ENC needs a valid path
                    ---------------
                    """);
            }
        }

        private static void Encrypt(string file, string key) 
        {
            try
            {
                Aes _aes = Aes.Create();

                byte[] fileBytes = File.ReadAllBytes(file);

                _aes.Key = DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = _aes.CreateEncryptor();

                byte[] encFile = encryptor.TransformFinalBlock(fileBytes, 0, fileBytes.Length);

                string newFile = file + ".enc";

                File.WriteAllBytes(newFile, encFile);

                FileHelper.RestrictPermisions(newFile);

                File.Delete(file);

                Console.WriteLine($"""
                    ---------------
                    - File encryption succeeded
                    - Save your password: {key}
                    ---------------
                    """);
            } 
            catch (CryptographicException)
            {
                Console.WriteLine($"""
                    ---------------
                    - SingleEncrypter could not encrypt the file: {file}
                    ---------------
                    """);
            }
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
