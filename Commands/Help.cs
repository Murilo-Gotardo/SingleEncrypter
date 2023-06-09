﻿namespace SingleEncrypter.Commands
{
    internal class Help : Command
    {
        public override void ExecuteCommand(string[] args)
        {
            Console.WriteLine("""

                ---------------
                - SE (software information)
                - HELP (help with commands)
                - ENC (encrypt a file)
                - DEC  (decrypt a file)
                - BYE/EXIT (close SingleEncrypter)
                ---------------

                """);
        }

        public override bool VerifyCommand(string[] args)
        {
            return args[0] == "help";
        }
    }
}
