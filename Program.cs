using SingleEncrypter.Commands;

namespace SingleEncrypter
{
    internal class Program
    {
        //removed args (parameter of main) for testing
        private static void Main()
        {
            List<Command> commands = new()
            {
                new Command(),
                new Help(),
                new Decryptor(),
                new Encryptor()
            };

            while (true)
            {
                Console.Write("SingleEncrypter> ");
                string[] args = Console.ReadLine().ToLower().Split(" ");

                if (args[0] == "exit" || args[0] == "bye") break;

                bool commandFound = false;

                foreach (Command command in commands)
                {
                    if (command.VerifyCommand(args))
                    {
                        command.ExecuteCommand(args);
                        commandFound = true;
                        break;
                    }
                    //TODO: (maybe) refactor this section
                    else
                    {
                        if (args[0] == "clear") 
                            Console.Clear();
                            commandFound = true;
                    }
                    //till here
                }

                if (!commandFound)
                {
                    Console.WriteLine("""
                        ---------------
                        Invalid command (type HELP)
                        ---------------
                        """);
                }
            }
        }
    }
}