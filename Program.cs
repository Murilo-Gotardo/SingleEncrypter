using SingleEncrypter.Commands;

namespace SingleEncrypter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Command> commands = new()
            {
                new Command(),
                new Help(),
                new Encryptor()
            };

            //string[] args = Console.ReadLine().Split(" ");

            if (commands[0].VerifyCommand(args) && args.Length >= 1)
            {
                for (int i = 1; i < commands.Count; i++)
                {
                    // TODO: implementar o inicializador dos comandos
                 
                    //commands[i].ExecuteCommand(args);
                    //break;
                }
            }
            else
            {
                commands[0].ExecuteCommand(args);
            }
        }
    }
}