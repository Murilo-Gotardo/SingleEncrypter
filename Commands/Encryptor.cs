using System.Security.Cryptography;
using System.Text;
using SingleEncrypter.Helper;
using SingleEncrypter.UI;
using System.Diagnostics;

namespace SingleEncrypter.Commands
{
    internal class Encryptor : Command
    {
        public override void ExecuteCommand(string[] args)
        {
            try
            {
                string path = Path.GetFullPath(args[1]);

                if (string.IsNullOrEmpty(args[2]))
                {
                    Console.WriteLine($"""

                        ---------------
                        - ENC needs a password
                        ---------------

                        """);
                }
                else if (!File.Exists(path))
                {
                    Console.WriteLine($"""

                        ---------------
                        - File does not exist 
                        - Path provided: {path}
                        ---------------

                        """);
                    
                } 
                else
                {
                    Encrypt(path, args[2]);
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
            Stopwatch _stopwatch = new();
           
            try
            {
                _stopwatch.Start();

                Aes _aes = Aes.Create();

                _aes.Key = DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                string encFile = file + ".enc";

                FileStream _inFileStreamReader = new(file, FileMode.Open, FileAccess.Read);
                FileStream _outFileStreamWriter = new(encFile, FileMode.OpenOrCreate, FileAccess.Write);
                _outFileStreamWriter.SetLength(0);

                CryptoStream _cryptoStream = new(_outFileStreamWriter, _aes.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] buffer = new byte[10485760];

                int bytesRead;

                long totalBytesRead = 0;

                Console.CursorVisible = false;

                unsafe
                {
                    fixed (byte* pBuffer = buffer)
                    {
                        while ((bytesRead = _inFileStreamReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            byte* pBufferPinned = pBuffer;

                            _cryptoStream.Write(buffer, 0, bytesRead);

                            ProgressBar.Update(totalBytesRead += bytesRead, _inFileStreamReader.Length);
                        }
                    }
                }

                Console.WriteLine("\n");

                Console.ResetColor();
                Console.CursorVisible = true;

                _cryptoStream.Close();
                _inFileStreamReader.Close();
                _outFileStreamWriter.Close();

                FileHelper.RestrictPermisions(encFile);

                File.Delete(file);

                _stopwatch.Stop();

                FinalMessageHelper.Message(1, _stopwatch, key);
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
            return args[0] == "enc";
        }
    }
}
