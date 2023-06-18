namespace SingleEncrypter.Commands
{
    internal class Command
    {
        public virtual string? CommandName { get; set; }

        public virtual string? Option { get; set; }

        public virtual void ExecuteCommand(string[] args)
        {   
            Console.WriteLine("""

                SingleEncripter 0.1.0-beta
                ---------------
                HELP (commands)

                """);        
        }

        public virtual bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "se" ? args[0] : "";
            return args[0] == "se";
        }
    }
}
