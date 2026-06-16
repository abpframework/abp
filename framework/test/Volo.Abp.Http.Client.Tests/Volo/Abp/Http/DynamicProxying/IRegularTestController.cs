using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace Volo.Abp.Http.DynamicProxying;

public interface IRegularTestController
{
    Task<int> IncrementValueAsync(int value);

    Task<string> GetPlainStringAsync();

    Task<string> GetProducesJsonStringAsync();

    Task<string> GetProducesTextStringAsync();

    Task<string> GetNullStringAsync();

    Task<string> GetProducesJsonNullStringAsync();

    Task<string> GetEmptyStringAsync();

    Task<string> GetEscapedStringAsync();

    Task<IRemoteStreamContent> DownloadIconAsync();

    Task<object> GetReferenceTypeObjectAsync();

    Task<byte[]> GetByteArrayAsync();

    Task GetException1Async();

    Task GetException2Async();

    Task<DateTime> GetWithDateTimeParameterAsync(DateTime dateTime1);

    Task<string> PostValueWithHeaderAndQueryStringAsync(string headerValue, string qsValue);

    Task<string> PostValueWithBodyAsync(string bodyValue);

    Task<Car> PostObjectWithBodyAsync(Car bodyValue);

    Task<Car> PostObjectWithQueryAsync(Car bodyValue);

    Task<Car> GetObjectWithUrlAsync(Car bodyValue);

    Task<Car> GetObjectandIdAsync(int id, Car bodyValue);

    Task<Car> GetObjectandFirstReleaseDateAsync(DateTime time, Car bodyValue);

    Task<Car> GetObjectandCountAsync(int count, Car bodyValue);

    Task<Car> GetObjectAndIdWithQueryAsync(int id, Car bodyValue);

    Task<string> PutValueWithBodyAsync(string bodyValue);

    Task<string> PatchValueWithBodyAsync(string bodyValue);

    Task<string> PutValueWithHeaderAndQueryStringAsync(string headerValue, string qsValue);

    Task<string> PatchValueWithHeaderAndQueryStringAsync(string headerValue, string qsValue);

    Task<int> DeleteByIdAsync(int id);

    Task<string> AbortRequestAsync(CancellationToken cancellationToken = default);

    Task<string> TimeOutRequestAsync();

}
