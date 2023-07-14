using SingleEncrypter.Helper;
using SingleEncrypter.UI;
using System.Diagnostics;
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

                if (string.IsNullOrEmpty(args[2]))
                {
                    Console.WriteLine($"""

                        ---------------
                        - The file requires a password to be decrypted
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
                    Decrypt(path, args[2]); 
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

        public static void Decrypt(string file, string key)
        {
            FileHelper.RestorePermissions(file);

            Stopwatch _stopwatch = new();

            try
            {
                _stopwatch.Start();

                Aes _aes = Aes.Create();

                _aes.Key = Encryptor.DeriveKey(key);
                _aes.IV = new byte[16];
                _aes.Mode = CipherMode.CBC;
                _aes.Padding = PaddingMode.PKCS7;

                string decFile = Path.ChangeExtension(file, "");

                FileStream _inFileStreamReader = new(file, FileMode.Open, FileAccess.Read);
                FileStream _outFileStreamWriter = new(decFile, FileMode.OpenOrCreate, FileAccess.Write);
                _outFileStreamWriter.SetLength(0);

                CryptoStream _cryptoStream = new(_outFileStreamWriter, _aes.CreateDecryptor(), CryptoStreamMode.Write);

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

                File.Delete(file);

                _stopwatch.Stop();

                FinalMessageHelper.Message(2, _stopwatch);
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
            return args[0] == "dec";
        }
    }
}
