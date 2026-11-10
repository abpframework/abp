using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Volo.Abp.Localization.VirtualFiles.Json;
using Volo.Abp.Testing;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.Localization;

public class VirtualFileLocalizationResourceContributor_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    private const string CountryNamesPath = "/Volo/Abp/Localization/TestResources/Base/CountryNames";

    private const string CultureIndexPath = "/Volo/Abp/Localization/TestResources/CultureIndex";

    [Fact]
    public void Initialize_Should_Not_Resolve_Services()
    {
        var serviceProvider = new RecordingServiceProvider(ServiceProvider);
        var resource = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value.Resources.Get("LocalizationTestCountryNames");
        var contributor = new JsonVirtualFileLocalizationResourceContributor("/Volo/Abp/Localization/TestResources/Base/CountryNames");

        contributor.Initialize(new LocalizationResourceInitializationContext(resource, serviceProvider));

        serviceProvider.RequestedTypes.ShouldBeEmpty();

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");
        serviceProvider.RequestedTypes.ShouldBe(new[] { typeof(IVirtualFileProvider) });
    }

    [Fact]
    public void The_First_Read_Should_Resolve_Services_Outside_Of_Its_Lock()
    {
        var resource = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value.Resources.Get("LocalizationTestCountryNames");
        var contributor = new JsonVirtualFileLocalizationResourceContributor("/Volo/Abp/Localization/TestResources/Base/CountryNames");
        var serviceProvider = new ReadingWhileResolvingServiceProvider(ServiceProvider, contributor);
        contributor.Initialize(new LocalizationResourceInitializationContext(resource, serviceProvider));

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");

        serviceProvider.OtherReadCompletedWhileResolving.ShouldBeTrue();
        serviceProvider.OtherRead.IsCompletedSuccessfully.ShouldBeTrue();
    }

    [Fact]
    public void Should_Only_Build_The_Dictionaries_Of_The_Requested_Cultures()
    {
        var fileProvider = new RecordingFileProvider(GetRequiredService<IVirtualFileProvider>());
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor(CountryNamesPath), fileProvider);

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");
        contributor.GetOrNull("tr", "USA").Value.ShouldBe("Amerika Birleşik Devletleri");
        contributor.GetOrNull("en", "Brazil").ShouldNotBeNull();
        contributor.GetOrNull("xx", "USA").ShouldBeNull();

        fileProvider.GetReopenedFileNames().ShouldBe(new[] { "en.json", "tr.json" }, ignoreOrder: true);
        fileProvider.GetOpenCount().ShouldBe(27 + 2);
    }

    [Fact]
    public void Should_Use_The_Culture_In_The_File_Instead_Of_The_File_Name()
    {
        var fileProvider = new RecordingFileProvider(GetRequiredService<IVirtualFileProvider>());
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor(CountryNamesPath), fileProvider);

        contributor.GetOrNull("fr", "USA").Value.ShouldBe("les états-unis d'Amérique");
        contributor.GetOrNull("FR", "USA").ShouldBeNull();

        fileProvider.GetReopenedFileNames().ShouldBe(new[] { "FR.json" });
    }

    [Fact]
    public async Task Should_Get_The_Same_Supported_Cultures_Without_Building_Dictionaries()
    {
        var fileProvider = new RecordingFileProvider(GetRequiredService<IVirtualFileProvider>());
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor(CountryNamesPath), fileProvider);
        var parseAllContributor = CreateContributor(new ParseAllJsonContributor(CountryNamesPath));

        var cultures = (await contributor.GetSupportedCulturesAsync()).ToList();

        cultures.ShouldBe((await parseAllContributor.GetSupportedCulturesAsync()).ToList());
        cultures.Count.ShouldBe(27);
        fileProvider.GetReopenedFileNames().ShouldBeEmpty();
    }

    [Fact]
    public async Task Supported_Cultures_Should_Be_Read_Only()
    {
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor(CountryNamesPath));

        var cultures = (ICollection<string>)await contributor.GetSupportedCulturesAsync();

        cultures.IsReadOnly.ShouldBeTrue();
        Should.Throw<NotSupportedException>(() => cultures.Clear());
    }

    [Fact]
    public async Task Should_Read_The_Culture_From_Any_Json_Layout()
    {
        var fileProvider = new RecordingFileProvider(GetRequiredService<IVirtualFileProvider>());
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor(CultureIndexPath), fileProvider);

        (await contributor.GetSupportedCulturesAsync()).ShouldBe(new[] { "fr", "en", "de", "tr" });

        contributor.GetOrNull("en", "Hello").Value.ShouldBe("Hello");
        contributor.GetOrNull("de", "Hello").Value.ShouldBe("Hallo");
        contributor.GetOrNull("fr", "Hello").Value.ShouldBe("Bonjour");

        fileProvider.GetReopenedFileNames().ShouldBe(new[] { "en.json", "texts-de.json", "Comments.json" }, ignoreOrder: true);
    }

    [Fact]
    public void Should_Merge_Split_Files_In_File_Name_Order()
    {
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor("/Volo/Abp/Localization/TestResources/FilesSplit"));

        contributor.GetOrNull("en", "Base.Id").Value.ShouldBe("Id");
        contributor.GetOrNull("en", "Book.Id").Value.ShouldBe("ISBN");
        contributor.GetOrNull("en", "ThisIsRequiredValue").Value.ShouldBe("This is required value (this will be used)");
    }

    [Fact]
    public void Should_Parse_All_Files_If_The_Culture_Of_A_File_Can_Not_Be_Read()
    {
        var fileProvider = new RecordingFileProvider(GetRequiredService<IVirtualFileProvider>());
        var contributor = CreateContributor(new ParseAllJsonContributor(CountryNamesPath), fileProvider);

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");
        contributor.GetOrNull("fr", "USA").Value.ShouldBe("les états-unis d'Amérique");

        fileProvider.GetOpenCount().ShouldBe(27);
    }

    [Fact]
    public void Should_Parse_All_Files_If_A_File_Does_Not_Have_The_Indexed_Culture()
    {
        var contributor = CreateContributor(new WrongCultureJsonContributor(CountryNamesPath));

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");
        contributor.GetOrNull("tr", "USA").Value.ShouldBe("Amerika Birleşik Devletleri");
        contributor.GetOrNull("fr", "USA").Value.ShouldBe("les états-unis d'Amérique");
    }

    [Fact]
    public void Should_Parse_All_Files_If_A_File_Can_Not_Be_Opened()
    {
        var fileProvider = new InMemoryFileProvider(new IFileInfo[] { new UnopenableFileInfo("en.json") });
        var contributor = CreateContributor(new GeneratedDictionaryJsonContributor("/Test"), new RecordingFileProvider(fileProvider));

        contributor.GetOrNull("en", "Hello").Value.ShouldBe("Hello");
    }

    [Fact]
    public void Should_Only_Build_A_Culture_Once_When_Read_Concurrently()
    {
        var fileProvider = new RecordingFileProvider(GetRequiredService<IVirtualFileProvider>());
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor(CountryNamesPath), fileProvider);

        Parallel.For(0, 32, _ => contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America"));

        fileProvider.GetReopenedFileNames().ShouldBe(new[] { "en.json" });
        fileProvider.GetOpenCount().ShouldBe(27 + 1);
    }

    [Fact]
    public async Task Should_Read_The_Files_Again_When_They_Change()
    {
        var fileProvider = new InMemoryFileProvider(new[]
        {
            CreateFile("en.json", "{\"culture\":\"en\",\"texts\":{\"Hello\":\"Hello\"}}"),
            CreateFile("fr.json", "{\"culture\":\"fr\",\"texts\":{\"Hello\":\"Bonjour\"}}")
        });
        var contributor = CreateContributor(new JsonVirtualFileLocalizationResourceContributor("/Test"), new RecordingFileProvider(fileProvider));

        contributor.GetOrNull("en", "Hello").Value.ShouldBe("Hello");
        contributor.GetOrNull("fr", "Hello").Value.ShouldBe("Bonjour");

        fileProvider.Change(
            CreateFile("de.json", "{\"culture\":\"de\",\"texts\":{\"Hello\":\"Hallo\"}}"),
            CreateFile("en.json", "{\"culture\":\"en\",\"texts\":{\"Hello\":\"Hi\"}}")
        );

        contributor.GetOrNull("en", "Hello").Value.ShouldBe("Hi");
        contributor.GetOrNull("fr", "Hello").ShouldBeNull();
        contributor.GetOrNull("de", "Hello").Value.ShouldBe("Hallo");
        (await contributor.GetSupportedCulturesAsync()).ShouldBe(new[] { "de", "en" });
    }

    private static IFileInfo CreateFile(string name, string json)
    {
        return new InMemoryFileInfo("/Test/" + name, Encoding.UTF8.GetBytes(json), name);
    }

    private TContributor CreateContributor<TContributor>(TContributor contributor, IVirtualFileProvider fileProvider = null)
        where TContributor : JsonVirtualFileLocalizationResourceContributor
    {
        var resource = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value.Resources.Get("LocalizationTestCountryNames");
        IServiceProvider serviceProvider = fileProvider == null ? ServiceProvider : new FileProviderServiceProvider(ServiceProvider, fileProvider);
        contributor.Initialize(new LocalizationResourceInitializationContext(resource, serviceProvider));
        return contributor;
    }

    private class ParseAllJsonContributor : JsonVirtualFileLocalizationResourceContributor
    {
        public ParseAllJsonContributor(string virtualPath)
            : base(virtualPath)
        {
        }

        protected override string GetCultureNameOrNull(IFileInfo file)
        {
            return null;
        }
    }

    private class WrongCultureJsonContributor : JsonVirtualFileLocalizationResourceContributor
    {
        public WrongCultureJsonContributor(string virtualPath)
            : base(virtualPath)
        {
        }

        protected override string GetCultureNameOrNull(IFileInfo file)
        {
            return "en";
        }
    }

    private class GeneratedDictionaryJsonContributor : JsonVirtualFileLocalizationResourceContributor
    {
        public GeneratedDictionaryJsonContributor(string virtualPath)
            : base(virtualPath)
        {
        }

        protected override ILocalizationDictionary CreateDictionaryFromFile(IFileInfo file)
        {
            return new StaticLocalizationDictionary("en", new Dictionary<string, LocalizedString>
            {
                { "Hello", new LocalizedString("Hello", "Hello") }
            });
        }
    }

    private class UnopenableFileInfo : IFileInfo
    {
        public bool Exists => true;
        public long Length => 0;
        public string PhysicalPath => null;
        public string Name { get; }
        public DateTimeOffset LastModified => DateTimeOffset.MinValue;
        public bool IsDirectory => false;

        public UnopenableFileInfo(string name)
        {
            Name = name;
        }

        public Stream CreateReadStream()
        {
            throw new FileNotFoundException(Name);
        }
    }

    private class FileProviderServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IVirtualFileProvider _fileProvider;

        public FileProviderServiceProvider(IServiceProvider serviceProvider, IVirtualFileProvider fileProvider)
        {
            _serviceProvider = serviceProvider;
            _fileProvider = fileProvider;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IVirtualFileProvider) ? _fileProvider : _serviceProvider.GetService(serviceType);
        }
    }

    private class RecordingFileProvider : IVirtualFileProvider
    {
        private readonly IFileProvider _fileProvider;
        private readonly ConcurrentQueue<string> _openedFileNames = new();

        public RecordingFileProvider(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public int GetOpenCount()
        {
            return _openedFileNames.Count;
        }

        /// <summary>
        /// The files opened more than once: all files are read once to index their cultures, then a file is opened again to build the dictionary of its culture.
        /// </summary>
        public List<string> GetReopenedFileNames()
        {
            return _openedFileNames.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new FileList(_fileProvider.GetDirectoryContents(subpath)
                .Select(x => (IFileInfo)new RecordingFileInfo(x, _openedFileNames))
                .ToList());
        }

        public IChangeToken Watch(string filter)
        {
            return _fileProvider.Watch(filter);
        }
    }

    private class InMemoryFileProvider : IFileProvider
    {
        private IFileInfo[] _files;
        private CancellationTokenSource _changeTokenSource = new();

        public InMemoryFileProvider(IFileInfo[] files)
        {
            _files = files;
        }

        public void Change(params IFileInfo[] files)
        {
            _files = files;
            var changeTokenSource = _changeTokenSource;
            _changeTokenSource = new CancellationTokenSource();
            changeTokenSource.Cancel();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return new NotFoundFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new FileList(_files.ToList());
        }

        public IChangeToken Watch(string filter)
        {
            return new CancellationChangeToken(_changeTokenSource.Token);
        }
    }

    private class FileList : IDirectoryContents
    {
        private readonly List<IFileInfo> _files;

        public bool Exists => true;

        public FileList(List<IFileInfo> files)
        {
            _files = files;
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return _files.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private class RecordingFileInfo : IFileInfo
    {
        private readonly IFileInfo _fileInfo;
        private readonly ConcurrentQueue<string> _openedFileNames;

        public bool Exists => _fileInfo.Exists;
        public long Length => _fileInfo.Length;
        public string PhysicalPath => _fileInfo.PhysicalPath;
        public string Name => _fileInfo.Name;
        public DateTimeOffset LastModified => _fileInfo.LastModified;
        public bool IsDirectory => _fileInfo.IsDirectory;

        public RecordingFileInfo(IFileInfo fileInfo, ConcurrentQueue<string> openedFileNames)
        {
            _fileInfo = fileInfo;
            _openedFileNames = openedFileNames;
        }

        public Stream CreateReadStream()
        {
            _openedFileNames.Enqueue(Name);
            return _fileInfo.CreateReadStream();
        }
    }

    /* Reads the same resource from another thread while the first read resolves the virtual file provider, as a
     * container does when it creates a singleton that reads localized texts while it holds its own lock. */
    private class ReadingWhileResolvingServiceProvider : IServiceProvider
    {
        public Task OtherRead { get; private set; }

        public bool OtherReadCompletedWhileResolving { get; private set; }

        private readonly IServiceProvider _serviceProvider;
        private readonly JsonVirtualFileLocalizationResourceContributor _contributor;
        private int _otherReadStarted;

        public ReadingWhileResolvingServiceProvider(IServiceProvider serviceProvider, JsonVirtualFileLocalizationResourceContributor contributor)
        {
            _serviceProvider = serviceProvider;
            _contributor = contributor;
        }

        public object GetService(Type serviceType)
        {
            // The other read resolves the provider too, and may do so before OtherRead is assigned.
            if (serviceType == typeof(IVirtualFileProvider) && Interlocked.CompareExchange(ref _otherReadStarted, 1, 0) == 0)
            {
                var otherRead = Task.Run(() => _contributor.GetOrNull("en", "Brazil"));
                OtherRead = otherRead;
                OtherReadCompletedWhileResolving = otherRead.Wait(TimeSpan.FromSeconds(5));
            }

            return _serviceProvider.GetService(serviceType);
        }
    }

    private class RecordingServiceProvider : IServiceProvider
    {
        public List<Type> RequestedTypes { get; } = new();

        private readonly IServiceProvider _serviceProvider;

        public RecordingServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            RequestedTypes.Add(serviceType);
            return _serviceProvider.GetService(serviceType);
        }
    }
}
