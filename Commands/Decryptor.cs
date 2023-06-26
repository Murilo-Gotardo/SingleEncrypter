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

            //Losing information (Padding is invalid and cannot be removed.)

            try
            {
                Aes _aes = Aes.Create();

                _aes.Key = Encryptor.DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                string decFile = Path.ChangeExtension(file, "");

                FileStream _inFileStreamReader = new(file, FileMode.Open, FileAccess.Read);
                FileStream _outFileStreamReader = new(decFile, FileMode.OpenOrCreate, FileAccess.Write);
                _outFileStreamReader.SetLength(0);

                CryptoStream _cryptoStream = new(_outFileStreamReader, _aes.CreateDecryptor(), CryptoStreamMode.Write);

                byte[] buffer = new byte[4096];

                int bytesRead;

                while ((bytesRead = _inFileStreamReader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _cryptoStream.Write(buffer, 0, bytesRead);
                }

                _cryptoStream.Close();
                _inFileStreamReader.Close();
                _outFileStreamReader.Close();

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
                    - Invalid password
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

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "dec" ? args[0] : "";
            return args[0] == "dec";
        }
    }
}
