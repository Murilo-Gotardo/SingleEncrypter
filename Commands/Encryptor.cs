using System.Security.Cryptography;

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
                    Encrypt(new FileInfo(path));
                }
                else
                {
                    Console.WriteLine($"""

                    Arquivo inválido: {path}

                    """);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ENC precisa de um caminho de arquivo", ex);
            }
        }

        private static void Encrypt(FileInfo file) 
        {         
            Aes aes = Aes.Create();

            ICryptoTransform transform = aes.CreateEncryptor();

            var _rsa = new RSACryptoServiceProvider();

            byte[] keyEncrypted = _rsa.Encrypt(aes.Key, false);

            int lKey = keyEncrypted.Length;
            byte[] LenK = BitConverter.GetBytes(lKey);
            int lIV = aes.IV.Length;
            byte[] LenIV = BitConverter.GetBytes(lIV);

            string outFile = Path.ChangeExtension(file.Name, ".enc");

            var outFs = new FileStream(outFile, FileMode.Create);
            
            outFs.Write(LenK, 0, 4);
            outFs.Write(LenIV, 0, 4);
            outFs.Write(keyEncrypted, 0, lKey);
            outFs.Write(aes.IV, 0, lIV);

            var outStreamEncrypted =
                new CryptoStream(outFs, transform, CryptoStreamMode.Write);
            
            int count = 0;
            int offset = 0;

            int blockSizeBytes = aes.BlockSize / 8;
            byte[] data = new byte[blockSizeBytes];
            int bytesRead = 0;

            var inFs = new FileStream(file.FullName, FileMode.Open);
            
            do
            {
                count = inFs.Read(data, 0, blockSizeBytes);
                offset += count;
                outStreamEncrypted.Write(data, 0, count);
                bytesRead += blockSizeBytes;
            } while (count > 0);
            
            outStreamEncrypted.FlushFinalBlock();

            inFs.Close();
            

            Console.WriteLine($"Arquivo ({file.Name}) criptografado");
        }

        public override bool VerifyCommand(string[] args)
        {
            return args[0] == "enc";
        }
    }
}
