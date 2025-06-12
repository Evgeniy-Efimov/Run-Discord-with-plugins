namespace RunDiscord.Helpers
{
    public static class IOHelper
    {
        public static List<FileInfo> FindFiles(string path, string fileName)
        {
            var directoryInfo = new DirectoryInfo(path);
            var results = directoryInfo.GetFiles().Where(
                f => f.Name.Trim().ToLower() == fileName.Trim().ToLower()).ToList();

            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                results.AddRange(FindFiles(subDirectory.FullName, fileName));
            }

            return results;
        }
    }
}
