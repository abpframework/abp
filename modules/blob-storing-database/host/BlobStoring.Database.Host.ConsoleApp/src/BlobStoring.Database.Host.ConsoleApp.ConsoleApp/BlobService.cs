using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp
{
    public class BlobService : IBlobService, ITransientDependency
    {
        private readonly IBlobContainer<ProfilePictureContainer> _container;

        public BlobService(IBlobContainer<ProfilePictureContainer> container)
        {
            _container = container;
        }

        public async Task SaveFile(string fileName = "File Name", string fileContent = "File Content")
        {
            await _container.SaveAsync(fileName, fileContent.GetBytes(), true);
            Console.WriteLine($"File: {fileName} is successfully saved");
        }
    }
}
