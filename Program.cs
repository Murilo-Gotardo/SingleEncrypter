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

                if (args[0] == "se")
                {
                    Console.Write("\n");
                    commands.ElementAt(0).ExecuteCommandAsync(args).GetAwaiter().GetResult();
                    Console.Write("\n");
                }
                else if (args[0] == "clear" || args[0] == "cls")
                {
                    Console.Clear();
                }
                else
                {
                    bool commandFound = false;

                    foreach (Command command in commands)
                    {
                        if (command.VerifyCommand(args))
                        {
                            Console.Write("\n");
                            command.ExecuteCommand(args);
                            Console.Write("\n");
                            commandFound = command.VerifyCommand(args);
                            break;
                        }
                    }

                    if (!commandFound)
                    {
                        Console.WriteLine("""

                            ---------------
                            - Invalid command (type HELP)
                            ---------------

                            """);
                        break;
                    }
                }
            }
        }
    }
}