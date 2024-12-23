namespace RunDiscord.Helpers
{
    public static class IOHelper
    {
        public static FileInfo FindFile(string path, string fileName)
        {

            var directoryInfo = new DirectoryInfo(path);
            var result = directoryInfo.GetFiles().FirstOrDefault(
                f => f.Name.ToLower() == fileName.ToLower());

            if (result == null)
            {
                foreach (var subDirectory in directoryInfo.GetDirectories())
                {
                    result = FindFile(subDirectory.FullName, fileName);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return result;
        }
    }
}
