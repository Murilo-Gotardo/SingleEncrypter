using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleEncrypter.Commands
{
    internal class Command
    {
        public virtual string CommandName { get; set; }

        public virtual string Option { get; set; }

        public virtual bool ExecuteCommand(string[] args)
        {
            if (VerifyCommand(args))
            {
                Console.WriteLine("""
                    MultiEncriper 0.1.0
                    ---------------
                    ME HELP para ajuda
                    """);

                return true;
            }
            else
            {
                return false;
            }             
        }

        public bool VerifyCommand(string[] args)
        {
            if (args[0] == "se")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
