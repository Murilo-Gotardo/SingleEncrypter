using SingleEncrypter.Commands;

namespace SingleEncrypter
{
    internal class Program
    {
        private static void Main()
        {
            List<Command> commands = new()
            {
                new Command(),
                new Help(),
                new Decryptor(),
                new Encryptor()
            };            

            for (; ;)
            {
                Console.Write("SingleEncrypter> ");
                string[] args = Console.ReadLine().Split(" ");

                if (args[0] == "exit" || args[0] == "bye")
                    break;
               
                for (int i = 0; i < commands.Count; i++)
                {
                    if (commands[i].VerifyCommand(args))
                    {
                        commands[i].ExecuteCommand(args);
                        break;
                    }
                }
            }  
        }
    }
}