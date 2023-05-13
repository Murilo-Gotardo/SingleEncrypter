namespace SingleEncrypter
{
    internal class FileHelper
    {
        // TODO: testar este código
        public static IEnumerable<string> FindFiles(
            string root,
            string defaultDir = "*.*")
        {
            var pendingPaths = new Queue<string>();
            var foundFiles = new List<string>();

            pendingPaths.Enqueue(root);

            while (pendingPaths.Count > 0)
            {
                var currentPath = pendingPaths.Dequeue();

                try
                {
                    var archiveList = Directory.GetFiles(currentPath, defaultDir);
                    foundFiles.AddRange(archiveList);

                    foreach (var subDir in Directory.GetDirectories(currentPath))
                        pendingPaths.Enqueue(subDir);
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignorar exceções sobre acesso não autorizado.
                }
            }

            return foundFiles;
        }
    }
}
