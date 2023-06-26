using SingleEncrypter.Commands;

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
                        command.ExecuteCommandAsync(args).GetAwaiter().GetResult();
                        commandFound = true;
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
                            command.ExecuteCommand(args);
                            commandFound = true;
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