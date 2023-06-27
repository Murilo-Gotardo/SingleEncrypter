using SingleEncrypter.Commands;
using SingleEncrypter.UI;

namespace SingleEncrypter
{
    internal class Program
    {
        private static void Main()
        {
            List<Command> commands = new()
            {
                new Core(),
                new Help(),
                new Decryptor(),
                new Encryptor()
            };

            while (true)
            {
                Console.Write("SingleEncrypter> ");
                
                string[]? args = Console.ReadLine()?.ToLower().Split(" ");

                if (args is null || args.Length == 0) continue;

                if (args[0] == "exit" || args[0] == "bye") break;

                bool commandFound = false;

                foreach (Command command in commands)
                {
                    if (args[0] == "se")
                    {
                        Console.Write("\n");
                        command.ExecuteCommandAsync(args).GetAwaiter().GetResult();
                        commandFound = true;
                        Console.Write("\n");
                        break;
                    }
                    else if (args[0] == "clear" || args[0] == "cls")
                    {
                        Console.Clear();
                        commandFound = true;
                        break;
                    }
                    else
                    {
                        if (command.VerifyCommand(args))
                        {
                            Console.Write("\n");
                            command.ExecuteCommand(args);
                            commandFound = true;
                            Console.Write("\n");
                            break;
                        }
                    }
                }

                if (!commandFound)
                {
                    Console.WriteLine("""
                        ---------------
                        - Invalid command (type HELP)
                        ---------------
                        """);
                }
            }
        }
    }
}