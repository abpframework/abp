using System.Threading.Tasks;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp
{
    public interface IBlobService
    {
        Task SaveFile(string fileName = "File Name", string fileContent = "File Content");
    }
}