using System.Net;
using System.Threading.Tasks;
using System.Text;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public class ObsClient : IObsClient
    {
        private const string HttpClientName = "ObsClientHttpClientName";

        private readonly IHttpClientFactory _httpClientFactory;

        protected string EndPoint { get; }

        protected string AccessKeyId { get; }

        protected string SecretAccessKey { get; }

        internal ObsClient(
            IServiceProvider serviceProvider,
            string endPoint,
            string accessKeyId,
            string secretAccessKey)
        {
            Check.NotNullOrWhiteSpace(endPoint, nameof(endPoint));
            Check.NotNullOrWhiteSpace(accessKeyId, nameof(accessKeyId));
            Check.NotNullOrWhiteSpace(secretAccessKey, nameof(secretAccessKey));
            _httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            EndPoint = endPoint;
            AccessKeyId = accessKeyId;
            SecretAccessKey = secretAccessKey;
        }

        public async Task CreateContainer(string containerName, string containerLocation)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            const string mediaType = "application/xml";
            var authorizationValue = GetAuthorizationValue(HttpMethod.Put, rfc1123DateTime, containerName, "/", mediaType);

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var data = $"<CreateBucketConfiguration><Location>{containerLocation}</Location></CreateBucketConfiguration>";
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = await httpClient.PutAsync(GetBaseAddress(containerName), content);
            var result = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
        }

        public async Task PutObjectAsync(string containerName, string blobName, Stream blobStream)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            const string mediaType = "text/plain";
            var authorizationValue = GetAuthorizationValue(HttpMethod.Put, rfc1123DateTime, containerName, $"/{blobName}", mediaType);

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.BaseAddress = GetBaseAddress(containerName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var content = new StreamContent(blobStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = await httpClient.PutAsync($"/{blobName}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Stream> GetObjectAsync(string containerName, string blobName)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            var authorizationValue = GetAuthorizationValue(HttpMethod.Get, rfc1123DateTime, containerName, $"/{blobName}");

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.BaseAddress = GetBaseAddress(containerName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var response = await httpClient.GetAsync($"/{blobName}");
            var stream = await response.Content.ReadAsStreamAsync();

            return stream;
        }

        public async Task DeleteObjectAsync(string containerName, string blobName)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            var authorizationValue = GetAuthorizationValue(HttpMethod.Delete, rfc1123DateTime, containerName, $"/{blobName}");

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.BaseAddress = GetBaseAddress(containerName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var response = await httpClient.DeleteAsync($"/{blobName}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            var authorizationValue = GetAuthorizationValue(HttpMethod.Delete, rfc1123DateTime, containerName, "/");

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.BaseAddress = GetBaseAddress(containerName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var response = await httpClient.DeleteAsync("/");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DoesContainerExistAsync(string containerName)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            var authorizationValue = GetAuthorizationValue(HttpMethod.Head, rfc1123DateTime, containerName, "/");

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var request = new HttpRequestMessage(HttpMethod.Head, GetBaseAddress(containerName));

            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> DoesBlobExistAsync(string containerName, string blobName)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            var authorizationValue = GetAuthorizationValue(HttpMethod.Head, rfc1123DateTime, containerName, $"/{blobName}");

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var request = new HttpRequestMessage(HttpMethod.Head, GetBaseAddress(containerName) + blobName);

            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<List<string>> GetObjectNamesAsync(string containerName)
        {
            Check.NotNullOrWhiteSpace(containerName, nameof(containerName));

            var rfc1123DateTime = DateTime.UtcNow.ToString("r");
            var authorizationValue = GetAuthorizationValue(HttpMethod.Get, rfc1123DateTime, containerName, "/");

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            httpClient.BaseAddress = GetBaseAddress(containerName);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationValue);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Date", rfc1123DateTime);

            var response = await httpClient.GetAsync("/");
            var result = await response.Content.ReadAsStringAsync();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);
            var nameNodes = xmlDocument.GetElementsByTagName("Key");
            var names = new List<string>();
            for (var i = 0; i < nameNodes.Count; i++)
            {
                names.Add(nameNodes[i].InnerText);
            }

            return names;
        }

        private string GetAuthorizationValue(HttpMethod httpMethod, string rfc1123DateTime, string containerName, string resource, string contentType = "")
        {
            var stringToSign =
            $"{httpMethod.Method.ToUpper()}\n\n{contentType}\n{rfc1123DateTime}\n/{containerName}{resource}";

            var bytes = Encoding.Default.GetBytes(stringToSign);
            var hmacsha1 = new HMACSHA1(Encoding.Default.GetBytes(SecretAccessKey));
            var hash = hmacsha1.ComputeHash(bytes);
            var signedString = Convert.ToBase64String(hash);

            return $"OBS {AccessKeyId}:{signedString}";
        }

        private Uri GetBaseAddress(string containerName)
        {
            var endPoint = new Uri(EndPoint);
            var uriBuilder = new UriBuilder
            {
                Scheme = endPoint.Scheme,
                Host = $"{containerName}.{endPoint.Host}"
            };

            return uriBuilder.Uri;
        }
    }
}