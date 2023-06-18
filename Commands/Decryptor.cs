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

                    File does not exist: {path}

                    """);
                }
        }

        public static void Decrypt(string file, string key)
        {
            Aes aes = Aes.Create();

            byte[] fileBytes = File.ReadAllBytes(file);

            aes.Key = Encryptor.DeriveKey(key);
            aes.IV = new byte[16];
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decrypt = aes.CreateDecryptor();

            byte[] decFile = decrypt.TransformFinalBlock(fileBytes, 0, fileBytes.Length);

            File.WriteAllBytes(Path.ChangeExtension(file, ""), decFile);
            File.Delete(file);

            //TODO: handle the exception

            //Exception:  The input data is not a complete block
            //the enc file has been modified
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "dec" ? args[0] : "";
            return args[0] == "dec";
        }
    }
}
