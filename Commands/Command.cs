namespace SingleEncrypter.Commands
{
    internal class Command
    {
        public virtual string? CommandName { get; set; }

        public virtual string? Option { get; set; }

        public virtual void ExecuteCommand(string[] args)
        {   
            Console.WriteLine("""
                MultiEncriper 0.1.0
                ---------------
                SE HELP para ajuda
                """);        
        }

        public virtual bool VerifyCommand(string[] args)
        {
            return args[0] == "se";
        }
    }
}
