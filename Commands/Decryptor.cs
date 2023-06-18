using SingleEncrypter.Helper;
using System.Security.Cryptography;

namespace SingleEncrypter.Commands
{
    internal class Decryptor : Command
    {
        public override void ExecuteCommand(string[] args)
        {
            try
            {
                string path = Path.GetFullPath(args[1]);

                if (File.Exists(path))
                {
                    Decrypt(path, args[2]);
                    Console.WriteLine($"""
                        ---------------
                        File decryption succeeded
                        ---------------
                        """);
                }
                else
                {
                    Console.WriteLine($"""
                    ---------------
                    File does not exist 
                    Path provided: {path}
                    ---------------
                    """);
                }                  
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"""
                    ---------------
                    DEC needs a valid path
                    ---------------
                    """);
            }
                
        }

        public static void Decrypt(string file, string key)
        {
            try
            {
                FileHelper.RestorePermissions(file);

                Aes _aes = Aes.Create();

                byte[] fileBytes = File.ReadAllBytes(file);

                _aes.Key = Encryptor.DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decrypt = _aes.CreateDecryptor();

                byte[] decFile = decrypt.TransformFinalBlock(fileBytes, 0, fileBytes.Length);

                File.WriteAllBytes(Path.ChangeExtension(file, ""), decFile);
                File.Delete(file);
            }
            catch (CryptographicException)
            {
                DateTime modifiedFile = File.GetLastWriteTime(file);

                Console.WriteLine($"""
                    ---------------
                    The file has been modified (impossible to decrypt)
                    Modified date: {modifiedFile}
                    ---------------
                    """);
            }
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "dec" ? args[0] : "";
            return args[0] == "dec";
        }
    }
}
