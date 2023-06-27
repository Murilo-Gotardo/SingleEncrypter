using SingleEncrypter.Helper;
using System.Reflection;

namespace SingleEncrypter.Commands
{
    internal class Core : Command
    {
        public override string? CommandName { get; set; }

        public override string? Option { get; set; }

        public override void ExecuteCommand(string[] args)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteCommandAsync(string[] args)
        {
            await GitHubTagHelper.GetLatestTag("Murilo-Gotardo", "SingleEncrypter");

            string? projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string cacheFilePath = Path.Combine(projectDirectory!, "Cache", "tagCache.txt");

            string tag = File.ReadAllText(cacheFilePath);

            Console.WriteLine($"""
                ***********************************
                # SingleEncripter {tag}           
                # ---------------                 
                # HELP (commands)                 
                ***********************************
                """);
          
        }

        public override bool VerifyCommand(string[] args)
        {
            CommandName = args[0] == "se" ? args[0] : "";
            return args[0] == "se";
        }
    }
}
