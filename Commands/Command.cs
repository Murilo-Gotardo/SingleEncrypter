using SingleEncrypter.Helper;
using System.Reflection;

namespace SingleEncrypter.Commands
{
    internal class Command
    {
        public virtual string? CommandName { get; set; }

        public virtual string? Option { get; set; }

        public virtual async void ExecuteCommand(string[] args)
        {
            GitHubTagHelper.GetLatestTag("Murilo-Gotardo", "SingleEncrypter");

            string projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           
            string cacheFilePath = Path.Combine(projectDirectory, "Cache", "tagCache.txt");
            
            string tag = File.ReadAllText(cacheFilePath);

            Console.WriteLine($"""
                ******************************
                # SingleEncripter {tag}      #
                # ---------------            #
                # HELP (commands)            #
                ******************************
                """);        
        }

        public virtual bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "se" ? args[0] : "";
            return args[0] == "se";
        }
    }
}
