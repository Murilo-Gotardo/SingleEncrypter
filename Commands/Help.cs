namespace SingleEncrypter.Commands
{
    internal class Help : Command
    {
        public override string? CommandName { get; set; }

        public override void ExecuteCommand(string[] args)
        {
            Console.WriteLine("""
                SE (faz coisas)
                """);
        }

        public override bool VerifyCommand(string[] args)
        {
            return args[1] == "help";
        }
    }
}
