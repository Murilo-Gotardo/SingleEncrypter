using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SingleEncrypter.Commands
{
    internal class Encryptor : Command
    {

        public override string CommandName { get; set; }

        public override string Option { get => base.Option; set => base.Option = value; }

        public override bool ExecuteCommand(string[] args)
        {
            try
            {
                // TODO: Arrumar o caminho absoluto (Ex: no momento ele retorna um arquivo que está em 'C:\dir\file.txt' apenas como 'C:\file.txt')
                string path = Path.GetFullPath(args[2]);

                if (File.Exists(path))
                {
                    Encrypt(path);
                    return true;
                }
                else
                {
                    Console.WriteLine($"""
                    Arquivo inválido
                    """);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ENC precisa de um caminho de arquivo", ex);
            }

        }

        public static void Encrypt(string path) 
        {
            string encryptedFilePath = Path.GetFileName(path);
            //string decryptedFilePath = "decrypted.txt";

            // Generate a random encryption key and IV
            byte[] key = new byte[32];
            byte[] iv = new byte[16];
            Aes aes = Aes.Create();

            aes.GenerateKey();
            aes.GenerateIV();
            Array.Copy(aes.Key, key, key.Length);
            Array.Copy(aes.IV, iv, iv.Length);


            // Encrypt the fil
            FileStream originalFileStream = new(path, FileMode.Open);
            FileStream encryptedFileStream = new(encryptedFilePath, FileMode.Create);
            CryptoStream cryptoStream = new(encryptedFileStream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write);

            originalFileStream.CopyTo(cryptoStream);


            //// Decrypt the file
            //using (FileStream encryptedFileStream = new FileStream(encryptedFilePath, FileMode.Open))
            //using (FileStream decryptedFileStream = new FileStream(decryptedFilePath, FileMode.Create))
            //using (CryptoStream cryptoStream = new CryptoStream(encryptedFileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            //{
            //    cryptoStream.CopyTo(decryptedFileStream);
            //}

            //Console.WriteLine("Encryption and decryption complete.");



            //base.GetCommand();
        }
    }
}
