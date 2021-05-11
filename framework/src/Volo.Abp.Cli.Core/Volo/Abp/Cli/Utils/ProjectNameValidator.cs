using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.Utils
{
    public static class ProjectNameValidator
    {
        private static readonly List<string> BannedProjectNames = new()
        {
            "MyCompanyName.MyProjectName", "MyProjectName", 
            "/", "?", ":", "&", "\\", "*", "\"", "<", ">", "|", "#", "%",
            "CON", "AUX", "PRN", "COM1", "LPT2", ".."
        };
        
        public static bool IsContainsBannedWord(string projectName)
        {
            return BannedProjectNames.Contains(projectName);
        }
        
        public static bool IsContainsControlOrSurrogateCharacter(string projectName)
        {
            return projectName.Any(chr => char.IsControl(chr) || char.IsSurrogate(chr));
        }
    }
}