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
                    Decrypt(new FileInfo(path));
                }
                else
                {
                    Console.WriteLine($"""

                    Arquivo inválido: {path}

                    """);
                }
        }

        public static void Decrypt(FileInfo file)
        {
            Aes aes = Aes.Create();

            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            string outFile =
                Path.ChangeExtension(file.FullName.Replace("Encrypt", "Decrypt"), ".txt");

            var inFs = new FileStream(file.FullName, FileMode.Open);
            
            inFs.Seek(0, SeekOrigin.Begin);
            inFs.Read(LenK, 0, 3);
            inFs.Seek(4, SeekOrigin.Begin);
            inFs.Read(LenIV, 0, 3);

            int lenK = BitConverter.ToInt32(LenK, 0);
            int lenIV = BitConverter.ToInt32(LenIV, 0);

            int startC = lenK + lenIV + 8;
            int lenC = (int)inFs.Length - startC;

            byte[] KeyEncrypted = new byte[lenK];
            byte[] IV = new byte[lenIV];

            inFs.Seek(8, SeekOrigin.Begin);
            inFs.Read(KeyEncrypted, 0, lenK);
            inFs.Seek(8 + lenK, SeekOrigin.Begin);
            inFs.Read(IV, 0, lenIV);

            var _rsa = new RSACryptoServiceProvider();
            byte[] KeyDecrypted = _rsa.Decrypt(KeyEncrypted, false);

            ICryptoTransform transform = aes.CreateDecryptor(KeyDecrypted, IV);

            var outFs = new FileStream(outFile, FileMode.Create);
                
            int count = 0;
            int offset = 0;

            int blockSizeBytes = aes.BlockSize / 8;
            byte[] data = new byte[blockSizeBytes];

            inFs.Seek(startC, SeekOrigin.Begin);
            var outStreamDecrypted =
                new CryptoStream(outFs, transform, CryptoStreamMode.Write);
                    
            do
            {
                count = inFs.Read(data, 0, blockSizeBytes);
                offset += count;
                outStreamDecrypted.Write(data, 0, count);
            } while (count > 0);

            outStreamDecrypted.FlushFinalBlock();
        }

        public override bool VerifyCommand(string[] args)
        {
            return args[0] == "dec";
        }
    }
}
