using Newtonsoft.Json;
using System.Reflection;

namespace SingleEncrypter.Helper
{
    internal class GitHubTagHelper
    {
        public static async Task GetLatestTag(string owner, string repo)
        { 
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "SingleEncrypter");

            var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/tags");
            response.EnsureSuccessStatusCode();

            var tagsJson = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<dynamic[]>(tagsJson);

            //TODO: Review the code, for some reason it is not working (while it is, in fact, working)
            if (tags!.Length > 0)
            {
                string latestTag = tags[0].name;

                string? projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                

                if (!Directory.Exists(Path.Combine(projectDirectory!, "Cache")))
                {
                    Directory.CreateDirectory(Path.Combine(projectDirectory!, "Cache"));
                    
                }

                string cacheDirectory = Path.Combine(projectDirectory!, "Cache");

                string cacheFilePath = Path.Combine(cacheDirectory, "tagCache.txt");

                File.WriteAllText(cacheFilePath, latestTag);
            }
        }
    }
}
