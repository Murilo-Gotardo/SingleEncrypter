namespace SingleEncrypter.Commands
{
    internal class Help : Command
    {
        public override string? CommandName { get; set; }

        public override string? Option { get; set; }

        public override void ExecuteCommand(string[] args)
        {
            Console.WriteLine("""
                ---------------
                - SE (software information)
                - HELP (help with commands)
                - ENC (encript a file)
                - DEC  (decript a file)
                - BYE/EXIT (close SingleEncrypter)
                ---------------
                """);
        }

        public override Task ExecuteCommandAsync(string[] args)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "help" ? args[0] : "";
            return args[0] == "help";
        }
    }
}
