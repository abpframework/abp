using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem;
using Volo.Abp.Storage.FileSystem.ExtendedProperties;
using Volo.Abp.Storage.FileSystem.Server;
using Xunit;

namespace Volo.Abp.Storage
{
    public class FileSystemStorage_Tests : AbpIntegratedTest<FileSystemStorage_Tests.TestModule>
    {
        private readonly IAbpStorageFactory _storageFactory;

        public FileSystemStorage_Tests()
        {
            _storageFactory = GetRequiredService<IAbpStorageFactory>();
        }

        #region Update

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Write_AllText(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var textToWrite = "The answer is 42";
            var filePath = "Update/42.txt";

            await store.SaveAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");

            var readFromWrittenFile = await store.ReadAllTextAsync(filePath);

            Assert.Equal(textToWrite, readFromWrittenFile);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task ETag_ShouldBe_TheSame_With_SameContent(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var textToWrite = "ETag Test Compute";
            var filePath = "Update/etag-same.txt";

            var savedReference = await store.SaveAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");
            var readReference = await store.GetAsync(filePath, withMetadata: true);

            Assert.Equal(savedReference.Properties.ETag, readReference.Properties.ETag);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task ETag_ShouldBe_Different_With_DifferentContent(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var textToWrite = "ETag Test Compute";
            var filePath = "Update/etag-different.txt";
            var textToUpdate = "ETag Test Compute 2";

            var savedReference = await store.SaveAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");
            var updatedReference = await store.SaveAsync(Encoding.UTF8.GetBytes(textToUpdate), filePath, "text/plain");

            Assert.NotEqual(savedReference.Properties.ETag, updatedReference.Properties.ETag);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Save_Stream(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var textToWrite = "The answer is 42";
            var filePath = "Update/42-2.txt";

            await store.SaveAsync(new MemoryStream(Encoding.UTF8.GetBytes(textToWrite)), filePath, "text/plain");

            var readFromWrittenFile = await store.ReadAllTextAsync(filePath);

            Assert.Equal(textToWrite, readFromWrittenFile);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Add_Metatada_Roundtrip(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetAsync(testFile, withMetadata: true);

            var id = Guid.NewGuid().ToString();

            file.Properties.Metadata.Add("newid", id);

            await file.SavePropertiesAsync();

            file = await store.GetAsync(testFile, withMetadata: true);

            var actualId = file.Properties.Metadata["newid"];

            Assert.Equal(id, actualId);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Save_Metatada_Roundtrip(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetAsync(testFile, withMetadata: true);

            var id = Guid.NewGuid().ToString();

            file.Properties.Metadata["id"] = id;

            await file.SavePropertiesAsync();

            file = await store.GetAsync(testFile, withMetadata: true);

            var actualId = file.Properties.Metadata["id"];

            Assert.Equal(id, actualId);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Save_Encoded_Metatada_Roundtrip(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetAsync(testFile, withMetadata: true);

            var name = "ï";

            file.Properties.Metadata["name"] = name;

            await file.SavePropertiesAsync();

            file = await store.GetAsync(testFile, withMetadata: true);

            var actualName = file.Properties.Metadata["name"];

            Assert.Equal(name, actualName);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_Metatada_Roundtrip(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetAsync(testFile, withMetadata: true);

            var id = Guid.NewGuid().ToString();

            file.Properties.Metadata["id"] = id;

            await file.SavePropertiesAsync();

            var files = await store.ListAsync("Metadata", withMetadata: true);

            string actualId = null;

            foreach (var aFile in files)
            {
                if (aFile.Path == testFile)
                {
                    actualId = aFile.Properties.Metadata["id"];
                }
            }

            Assert.Equal(id, actualId);
        }

        #endregion

        #region List

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_RootFiles(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new[] { "TextFile.txt", "template.hbs" };

            var results = await store.ListAsync(null);

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_EmptyPathFiles(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new string[] { "TextFile.txt", "template.hbs" };

            var results = await store.ListAsync("");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_SubDirectoryFiles(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new[] { "SubDirectory/TextFile2.txt" };

            var results = await store.ListAsync("SubDirectory");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_SubDirectoryFiles_WithTrailingSlash(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new[] { "SubDirectory/TextFile2.txt" };

            var results = await store.ListAsync("SubDirectory/");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_By_ExtensionGlobbing(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new[] { "Globbing/template.hbs", "Globbing/template-header.hbs" };

            var results = await store.ListAsync("Globbing", "*.hbs");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_By_FileNameGlobbing(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new[] { "Globbing/template.hbs", "Globbing/template.mustache" };

            var results = await store.ListAsync("Globbing", "template.*");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_By_FileNameGlobbingAtRoot(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expected = new[] { "template.hbs" };

            var results = await store.ListAsync("", "template.*");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            missingFiles.ShouldBeEmpty();
            unexpectedFiles.ShouldBeEmpty();
        }

        #endregion

        #region Read

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Read_AllText_From_RootFile(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = "42";

            var actualText = await store.ReadAllTextAsync("TextFile.txt");

            Assert.Equal(expectedText, actualText);
        }

        [InlineData("FileSystemTestStore1")]

        [Theory]
        public async Task Read_AllText_From_SubdirectoryFile(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var actualText = await store.ReadAllTextAsync("SubDirectory/TextFile2.txt");

            Assert.Equal(expectedText, actualText);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Read_AllByte_From_SubdirectoryFile(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = ">42";

            using (var reader = new StreamReader(new MemoryStream(
                await store.ReadAllBytesAsync("SubDirectory/TextFile2.txt")
                )))
            {
                var actualText = reader.ReadToEnd();
                Assert.Equal(expectedText, actualText);
            }
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Read_AllByte_From_SubdirectoryFile_Using_FileReference(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var file = await store.GetAsync("SubDirectory/TextFile2.txt");

            using (var reader = new StreamReader(new MemoryStream(await file.ReadAllBytesAsync())))
            {
                var actualText = reader.ReadToEnd();
                Assert.Equal(expectedText, actualText);
            }
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Read_File_From_SubdirectoryFile(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var file = await store.GetAsync("SubDirectory/TextFile2.txt");

            string actualText = null;

            using (var reader = new StreamReader(await file.ReadAsync()))
            {
                actualText = await reader.ReadToEndAsync();
            }

            Assert.Equal(expectedText, actualText);
        }

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Read_AllText_From_SubdirectoryFile_Using_FileReference(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var file = await store.GetAsync("SubDirectory/TextFile2.txt");

            string actualText = await file.ReadAllTextAsync();

            Assert.Equal(expectedText, actualText);
        }


        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task List_Then_Read_AllText_From_SubdirectoryFile(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var files = await store.ListAsync("SubDirectory");

            foreach (var file in files)
            {
                string actualText = await store.ReadAllTextAsync(file);

                Assert.Equal(expectedText, actualText);
            }
        }

        #endregion

        #region Delete

        [InlineData("FileSystemTestStore1")]
        [Theory]
        public async Task Delete_File_Test(string storeName)
        {
            var store = _storageFactory.GetStore(storeName);

            var file = await store.GetAsync("Delete/ToDelete.txt");

            await file.DeleteAsync();

            Assert.Null(await store.GetAsync("Delete/ToDelete.txt"));
            Assert.NotNull(await store.GetAsync("Delete/ToSurvive.txt"));
        }

        #endregion


        [DependsOn(
            typeof(AbpFileSystemStorageModule),
            typeof(AbpFileSystemStorageExtendedPropertiesModule),
            typeof(AbpFileSystemStorageServerModule))]
        public class TestModule : AbpModule
        {
            public override void PreConfigureServices(ServiceConfigurationContext context)
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true);

                var config = builder.Build();

                context.Services.SetConfiguration(config);
            }

            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                context.Services.Configure<AbpStorageOptions>(context.Services.GetConfiguration().GetSection("AppStorage"));
            }
        }
    }
}