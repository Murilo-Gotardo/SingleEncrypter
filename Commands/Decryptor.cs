using System.Security.Cryptography;

namespace SingleEncrypter.Commands
{
    internal class Decryptor : Command
    {
        public override void ExecuteCommand(string[] args)
        {
                string path = Path.GetFullPath(args[1]);

                if (File.Exists(path))
                {
                    Decrypt(path, args[2]);
                }
                else
                {
                    Console.WriteLine($"""

                    Arquivo inválido: {path}

                    """);
                }
        }

        public static void Decrypt(string file, string key)
        {
            Aes aes = Aes.Create();

            byte[] fileBytes = File.ReadAllBytes(file);

            aes.Key = Encryptor.DeriveKey(key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decrypt = aes.CreateDecryptor();

            byte[] decFile = decrypt.TransformFinalBlock(fileBytes, 0, fileBytes.Length);

            //TODO: keep the original extension

            File.WriteAllBytes(Path.ChangeExtension(file, ".txt"), decFile);
            File.Delete(file);
        }

        public override bool VerifyCommand(string[] args)
        {
            return args[0] == "dec";
        }
    }
}
