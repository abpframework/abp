namespace Volo.Abp.Cli.LIbs
{
    public class FileMatchResult
    {
        public string Path { get; }

        public string Stem { get; }

        public FileMatchResult(string path, string stem)
        {
            Path = path;
            Stem = stem;
        }
    }
}
