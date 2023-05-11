using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleEncrypter.Commands
{
    internal class Help : Command
    {

        public override string CommandName { get; set; }

        public override bool ExecuteCommand(string[] args)
        {
            if (args[1] == "help")
            {
                HelpUser();

                return true;
            }
            else
            {
                return false;
            }      
        }

        public static void HelpUser()
        {
            Console.WriteLine("""
                ME (faz coisas)
                """);
        } 
    }
}
