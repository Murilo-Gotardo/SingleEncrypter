using SingleEncrypter.Helper;
using System.Reflection;

namespace SingleEncrypter.Commands
{
    internal class Command
    {
        public virtual void ExecuteCommand(string[] args) { }

        public static async Task ExecuteCommandAsync(string[] args)
        {
            await GitHubTagHelper.GetLatestTag("Murilo-Gotardo", "SingleEncrypter");

            string? projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string cacheFilePath = Path.Combine(projectDirectory!, "Cache", "tagCache.txt");

            string tag = File.ReadAllText(cacheFilePath);

            Console.WriteLine($"""

                ***********************************
                # SingleEncrypter {tag}           
                # ---------------                 
                # HELP (commands)                 
                ***********************************

                """);
        }

        public virtual bool VerifyCommand(string[] args)
        {
            return args[0] == "se";
        }
    }
}
