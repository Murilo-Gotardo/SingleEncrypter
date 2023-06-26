using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using SingleEncrypter.Helper;

namespace SingleEncrypter.Commands
{
    internal class Encryptor : Command
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

        public override Task ExecuteCommandAsync(string[] args)
        {
            throw new NotImplementedException();
        }

        private static void Encrypt(string file, string key) 
        {
            try
            {
                Aes _aes = Aes.Create();

                _aes.Key = DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                string encFile = file + ".enc";

                FileStream _inFileStreamReader = new(file, FileMode.Open, FileAccess.Read);
                FileStream _outFileStreamReader = new(encFile, FileMode.OpenOrCreate, FileAccess.Write);
                _outFileStreamReader.SetLength(0);

                CryptoStream _cryptoStream = new(_outFileStreamReader, _aes.CreateEncryptor(), CryptoStreamMode.Write);                

                byte[] buffer = new byte[4096];

                int bytesRead;

                while ((bytesRead = _inFileStreamReader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _cryptoStream.Write(buffer, 0, bytesRead);
                }

                _cryptoStream.Close();
                _inFileStreamReader.Close();
                _outFileStreamReader.Close();

                FileHelper.RestrictPermisions(encFile);

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

        public static byte[] DeriveKey(string key)
        {
            byte[] privateKeyBytes = Encoding.UTF8.GetBytes(key);
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
