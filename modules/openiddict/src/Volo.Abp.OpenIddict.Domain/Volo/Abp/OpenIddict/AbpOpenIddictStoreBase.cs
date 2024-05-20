using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict;

public abstract class AbpOpenIddictStoreBase<TRepository>
    where TRepository : IRepository
{
    public ILogger<AbpOpenIddictStoreBase<TRepository>> Logger { get; set; }

    protected TRepository Repository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }
    protected IOpenIddictDbConcurrencyExceptionHandler ConcurrencyExceptionHandler { get; }
    protected IOptions<AbpOpenIddictStoreOptions> StoreOptions { get; }

    protected AbpOpenIddictStoreBase(TRepository repository, IUnitOfWorkManager unitOfWorkManager, IGuidGenerator guidGenerator, AbpOpenIddictIdentifierConverter identifierConverter, IOpenIddictDbConcurrencyExceptionHandler concurrencyExceptionHandler, IOptions<AbpOpenIddictStoreOptions> storeOptions)
    {
        Repository = repository;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;
        IdentifierConverter = identifierConverter;
        ConcurrencyExceptionHandler = concurrencyExceptionHandler;
        StoreOptions = storeOptions;

        Logger = NullLogger<AbpOpenIddictStoreBase<TRepository>>.Instance;
    }

    protected virtual Guid ConvertIdentifierFromString(string identifier)
    {
        return IdentifierConverter.FromString(identifier);
    }

    protected virtual string ConvertIdentifierToString(Guid identifier)
    {
        return IdentifierConverter.ToString(identifier);
    }

    protected virtual string WriteStream(Action<Utf8JsonWriter> action)
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                   {
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                       Indented = false
                   }))
            {
                action(writer);
                writer.Flush();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }

    protected virtual async Task<string> WriteStreamAsync(Func<Utf8JsonWriter, Task> func)
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                   {
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                       Indented = false
                   }))
            {
                await func(writer);
                await writer.FlushAsync();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
