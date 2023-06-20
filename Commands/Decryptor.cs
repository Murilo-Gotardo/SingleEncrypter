using SingleEncrypter.Helper;
using System.Security.Cryptography;

namespace SingleEncrypter.Commands
{
    internal class Decryptor : Command
    {
        public override string? CommandName { get; set; }

        public override string? Option { get; set; }

        public override void ExecuteCommand(string[] args)
        {
            try
            {
                string path = Path.GetFullPath(args[1]);

                if (File.Exists(path))
                {
                    Decrypt(path, args[2]);
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
                    - DEC needs a valid path
                    ---------------
                    """);
            }
                
        }

        public override Task ExecuteCommandAsync(string[] args)
        {
            throw new NotImplementedException();
        }

        public static void Decrypt(string file, string key)
        {
            FileHelper.RestorePermissions(file);

            if (VerifyPassword(file, key))
            {
                try
                {
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

                    Console.WriteLine($"""
                    ---------------
                    - File decryption succeeded
                    ---------------
                    """);
                }
                catch (CryptographicException)
                {
                    DateTime modifiedFile = File.GetLastWriteTime(file);

                    Console.WriteLine($"""
                    ---------------
                    - The file has been modified (impossible to decrypt)
                    - Modified date: {modifiedFile}
                    ---------------
                    """);
                }
                catch (Exception)
                {
                    Console.WriteLine($"""
                        ---------------
                        - Some unknown error ocurred while decrypting the file
                        ---------------
                        """);
                }
            }
        }

        public static bool VerifyPassword(string file, string key)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(file);

                Aes _aes = Aes.Create();

                _aes.Key = Encryptor.DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                MemoryStream ms = new(fileBytes);
                CryptoStream cs = new(ms, _aes.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader reader = new(cs);

                string decryptedContent = reader.ReadToEnd();

                return true;
            }
            catch (CryptographicException)
            {
                Console.WriteLine($"""
                    ---------------
                    - Invalid password
                    ---------------
                    """);

                return false;
            }
            catch (Exception)
            {
                Console.WriteLine($"""
                    ---------------
                    - Some error ocurred while verifing the password
                    ---------------
                    """);

                return false;
            }
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "dec" ? args[0] : "";
            return args[0] == "dec";
        }
    }
}
