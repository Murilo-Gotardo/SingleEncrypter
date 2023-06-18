namespace SingleEncrypter.Commands
{
    internal class Help : Command
    {
        public override string? CommandName { get; set; }

        public override void ExecuteCommand(string[] args)
        {
            Console.WriteLine("""
                ---------------
                SE (software information)
                HELP (commands)
                ENC (encript a file)
                DEC  (decript a file)
                ---------------
                """);
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "help" ? args[0] : "";
            return args[0] == "help";
        }
    }
}
