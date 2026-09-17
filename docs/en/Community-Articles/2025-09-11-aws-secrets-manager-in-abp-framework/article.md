# Step-by-Step AWS Secrets Manager Integration in ABP Framework Projects

## Introduction
In this article, we are going to discuss how to secure sensitive data in ABP Framework projects using AWS Secrets Manager and explain various aspects and concepts of _secret_ data management. We will explain step-by-step AWS Secrets Manager integration.


## What is the Problem?
Modern applications must store sensitive data such as API keys, database connection strings, OAuth client credentials, and other similar sensitive data. These are at the center of functionality but if stored in the wrong place can be massive security issues.

At build time, the first place that comes to mind is usually **appsettings.json**. This is a configuration file; it is not a secure place to store secret information, especially in production.

### Common Security Risks:
-  **Plain text storage**: Plain text storage of passwords
-  **Exposure to version control**: Secrets are rendered encrypted in Git repositories
-  **No access control**: Anyone who has file access can see the secrets
-  **No rotation**: We must change them manually
-  **No audit trail**: Who accessed which secret when is not known

## .NET User Secrets Tool vs AWS Secrets Manager

**User Secrets (.NET Secret Manager Tools)** is a dev environment only, local file-based solution that keeps sensitive information out of the repository.

**AWS Secrets Manager** is production. It's a centralized, encrypted, and audited secret management service.

| Feature                | User Secrets (Dev)           | AWS Secrets Manager (Prod)    |
| ---------------------- | ---------------------------- | ------------------------------ |
| Scope                  | Local developer machine      | All environments (dev/stage/prod) |
| Storage                | JSON in user profile         | Managed service (centralized) |
| Encryption             | None (plain text file)       | Encrypted with KMS            |
| Access Control         | OS file permissions          | IAM policies                  |
| Rotation               | None                         | Yes (automatic)               |
| Audit / Traceability   | None                         | Yes (CloudTrail)              |
| Typical Usage          | Quick dev outside repo       | Production secret management  |

---

## AWS Secrets Manager
Especially designed to securely store and handle sensitive and confidential data for our applications. It even supports features such as secret rotation, replication, and many more.

AWS Secrets Manager offers a trial of 30 days. After that, there is a $0.40 USD/month charge per stored secret. There is also a $0.05 USD fee per 10,000 API requests.

### Key Features:
-  **Automatic encryption**: KMS automatic encryption
-  **Automatic rotation**: Scheduled secret rotation
-  **Fine-grained access control**: IAM fine-grained access control
-  **Audit logging**: Full audit logging with CloudTrail
-  **Cross-region replication**: Cross-region replication
-  **API integration**: Programmatic access support

---

## Step 1: AWS Secrets Manager Setup

### 1.1 Creating a Secret in AWS Console
First, search for the Secrets Manager service in the AWS Management Console.

1. **AWS Console** → **Secrets Manager** → **Store a new secret**
2. Select **Secret type**:
   - **Other type of secret** (For custom key-value pairs)
   - **Credentials for RDS database** (For databases)
   - **Credentials for DocumentDB database**
   - **Credentials for Redshift cluster**



3. Enter **Secret value**:
```json
{
  "ConnectionString": "Server=myserver;Database=mydb;User Id=myuser;Password=mypassword;"
}
```

4. Set **Secret name**: `prod/ABPAWSTest/ConnectionString`
5. Add **Description**: "ABP Framework connection string for production"
6. Choose **Encryption key** (default KMS key is sufficient)
7. Configure **Automatic rotation** settings (optional)

### 1.2 IAM Permissions
Create an IAM policy for secret access:

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "secretsmanager:GetSecretValue",
                "secretsmanager:DescribeSecret"
            ],
            "Resource": "arn:aws:secretsmanager:eu-north-1:588118819172:secret:prod/ABPAWSTest/ConnectionString-*"
        }
    ]
}
```

---

## Step 2: ABP Framework Project Setup

### 2.1 NuGet Packages
Add the required AWS packages to your project:

```bash
dotnet add package AWSSDK.SecretsManager
dotnet add package AWSSDK.Extensions.NETCore.Setup
```

### 2.2 Configuration Files

**appsettings.json** (Development):
```json
{
  "AWS": {
    "Profile": "default",
    "Region": "eu-north-1",
    "AccessKey": "YOUR_ACCESS_KEY",
    "SecretKey": "YOUR_SECRET_KEY"
  },
  "SecretsManager": {
    "SecretName": "prod/ABPAWSTest/ConnectionString",
    "SecretArn": "arn:aws:secretsmanager:eu-north-1:588118819172:secret:prod/ABPAWSTest/ConnectionString-xtYQxv"
  }
}
```

**appsettings.Production.json** (Production):
```json
{
  "AWS": {
    "Region": "eu-north-1"
    // Use environment variables or IAM roles in production
  },
  "SecretsManager": {
    "SecretName": "prod/ABPAWSTest/ConnectionString"
  }
}
```

### 2.3 Environment Variables (Production)
```bash
export AWS_ACCESS_KEY_ID=your_access_key
export AWS_SECRET_ACCESS_KEY=your_secret_key
export AWS_DEFAULT_REGION=eu-north-1
```

---

## Step 3: AWS Integration Implementation

### 3.1 Program.cs Configuration

```csharp
using Amazon;
using Amazon.SecretsManager;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // AWS Secrets Manager configuration
        var awsOptions = builder.Configuration.GetAWSOptions();
        
        // Read AWS credentials from appsettings
        var accessKey = builder.Configuration["AWS:AccessKey"];
        var secretKey = builder.Configuration["AWS:SecretKey"];
        var region = builder.Configuration["AWS:Region"];
        
        if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
        {
            awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
        }
        
        if (!string.IsNullOrEmpty(region))
        {
            awsOptions.Region = RegionEndpoint.GetBySystemName(region);
        }
        
        builder.Services.AddDefaultAWSOptions(awsOptions);
        builder.Services.AddAWSService<IAmazonSecretsManager>();
        
        // ... ABP configuration
        await builder.AddApplicationAsync<YourAppModule>();
        var app = builder.Build();
        
        await app.InitializeApplicationAsync();
        await app.RunAsync();
    }
}
```

### 3.2 Secrets Manager Service

**Interface:**
```csharp
public interface ISecretsManagerService
{
    Task<string> GetSecretAsync(string secretName);
    Task<T> GetSecretAsync<T>(string secretName) where T : class;
    Task<string> GetConnectionStringAsync();
}
```

**Implementation:**
```csharp
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Volo.Abp.DependencyInjection;
using System.Text.Json;

public class SecretsManagerService : ISecretsManagerService, IScopedDependency
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SecretsManagerService> _logger;

    public SecretsManagerService(
        IAmazonSecretsManager secretsManager,
        IConfiguration configuration,
        ILogger<SecretsManagerService> logger)
    {
        _secretsManager = secretsManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        try
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            var response = await _secretsManager.GetSecretValueAsync(request);
            
            _logger.LogInformation("Successfully retrieved secret: {SecretName}", secretName);
            
            return response.SecretString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve secret: {SecretName}", secretName);
            throw;
        }
    }

    public async Task<T> GetSecretAsync<T>(string secretName) where T : class
    {
        var secretValue = await GetSecretAsync(secretName);
        
        try
        {
            return JsonSerializer.Deserialize<T>(secretValue) 
                ?? throw new InvalidOperationException($"Failed to deserialize secret {secretName}");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize secret {SecretName}", secretName);
            throw;
        }
    }

    public async Task<string> GetConnectionStringAsync()
    {
        var secretName = _configuration["SecretsManager:SecretName"] 
            ?? throw new InvalidOperationException("SecretsManager:SecretName configuration is missing");
            
        return await GetSecretAsync(secretName);
    }
}
```

---

## Step 4: Usage Examples

### 4.1 Using in Application Service

```csharp
[RemoteService(false)]
public class DatabaseService : ApplicationService
{
    private readonly ISecretsManagerService _secretsManager;
    
    public DatabaseService(ISecretsManagerService secretsManager)
    {
        _secretsManager = secretsManager;
    }
    
    public async Task<string> GetDatabaseConnectionAsync()
    {
        // Get connection string from AWS Secrets Manager
        var connectionString = await _secretsManager.GetConnectionStringAsync();
        
        // Use the connection string
        return connectionString;
    }
    
    public async Task<ApiConfiguration> GetApiConfigAsync()
    {
        // Deserialize JSON secret
        var config = await _secretsManager.GetSecretAsync<ApiConfiguration>("prod/MyApp/ApiConfig");
        
        return config;
    }
}
```

### 4.2 DbContext Configuration

```csharp
public class YourDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<YourDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<YourDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}

// Usage in Module
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();
    var secretsManager = context.Services.GetRequiredService<ISecretsManagerService>();
    
    // Get secret at startup and pass to DbContext
    var connectionString = await secretsManager.GetConnectionStringAsync();
    
    context.Services.AddAbpDbContext<YourDbContext>(options =>
    {
        options.AddDefaultRepositories(includeAllEntities: true);
        options.DbContextOptions.UseSqlServer(connectionString);
    });
}
```

---

## Step 5: Best Practices & Security

### 5.1 Security Best Practices

1. **Environment-based Configuration:**
   - Development: appsettings.json
   - Production: Environment variables or IAM roles

2. **Principle of Least Privilege:**
   ```json
   {
     "Effect": "Allow",
     "Action": "secretsmanager:GetSecretValue",
     "Resource": "arn:aws:secretsmanager:region:account:secret:specific-secret-*"
   }
   ```

3. **Secret Rotation:**
   - Set up automatic rotation
   - Custom rotation logic with Lambda functions

4. **Caching Strategy:**
   ```csharp
   public class CachedSecretsManagerService : ISecretsManagerService
   {
       private readonly IMemoryCache _cache;
       private readonly SecretsManagerService _secretsManager;
       
       public async Task<string> GetSecretAsync(string secretName)
       {
           var cacheKey = $"secret:{secretName}";
           
           if (_cache.TryGetValue(cacheKey, out string cachedValue))
           {
               return cachedValue;
           }
           
           var value = await _secretsManager.GetSecretAsync(secretName);
           
           _cache.Set(cacheKey, value, TimeSpan.FromMinutes(30));
           
           return value;
       }
   }
   ```

### 5.2 Error Handling

```csharp
public async Task<string> GetSecretWithRetryAsync(string secretName)
{
    const int maxRetries = 3;
    var delay = TimeSpan.FromSeconds(1);
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            return await GetSecretAsync(secretName);
        }
        catch (AmazonSecretsManagerException ex) when (i < maxRetries - 1)
        {
            _logger.LogWarning(ex, "Retry {Attempt} for secret {SecretName}", i + 1, secretName);
            await Task.Delay(delay);
            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2); // Exponential backoff
        }
    }
    
    throw new InvalidOperationException($"Failed to retrieve secret {secretName} after {maxRetries} attempts");
}
```

### 5.3 Performance Optimization

```csharp
public class PerformantSecretsManagerService : ISecretsManagerService
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PerformantSecretsManagerService> _logger;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<string> GetSecretAsync(string secretName)
    {
        var cacheKey = $"secret:{secretName}";
        
        // Try to get from cache first
        if (_cache.TryGetValue(cacheKey, out string cachedValue))
        {
            return cachedValue;
        }

        // Use semaphore to prevent multiple concurrent requests for the same secret
        await _semaphore.WaitAsync();
        try
        {
            // Double-check pattern
            if (_cache.TryGetValue(cacheKey, out cachedValue))
            {
                return cachedValue;
            }

            // Fetch from AWS
            var value = await GetSecretFromAwsAsync(secretName);
            
            // Cache for 30 minutes
            _cache.Set(cacheKey, value, TimeSpan.FromMinutes(30));
            
            return value;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
```

---

## Step 6: Testing & Debugging

### 6.1 Unit Testing

```csharp
public class SecretsManagerServiceTests : AbpIntegratedTest<TestModule>
{
    private readonly ISecretsManagerService _secretsManager;
    
    public SecretsManagerServiceTests()
    {
        _secretsManager = GetRequiredService<ISecretsManagerService>();
    }
    
    [Fact]
    public async Task Should_Get_Connection_String()
    {
        // Act
        var connectionString = await _secretsManager.GetConnectionStringAsync();
        
        // Assert
        connectionString.ShouldNotBeNullOrEmpty();
        connectionString.ShouldContain("Server=");
    }
    
    [Fact]
    public async Task Should_Deserialize_Json_Secret()
    {
        // Arrange
        var secretName = "test/json/config";
        
        // Act
        var config = await _secretsManager.GetSecretAsync<TestConfig>(secretName);
        
        // Assert
        config.ShouldNotBeNull();
        config.ApiKey.ShouldNotBeNullOrEmpty();
    }
}
```

### 6.2 Mock Implementation for Testing

```csharp
public class MockSecretsManagerService : ISecretsManagerService, ISingletonDependency
{
    private readonly Dictionary<string, string> _secrets = new()
    {
        ["prod/ABPAWSTest/ConnectionString"] = "Server=localhost;Database=TestDb;Trusted_Connection=true;",
        ["prod/MyApp/ApiKey"] = "test-api-key",
        ["prod/MyApp/Config"] = """{"ApiUrl": "https://api.test.com", "Timeout": 30}"""
    };

    public Task<string> GetSecretAsync(string secretName)
    {
        if (_secrets.TryGetValue(secretName, out var secret))
        {
            return Task.FromResult(secret);
        }
        
        throw new ArgumentException($"Unknown secret: {secretName}");
    }
    
    public async Task<T> GetSecretAsync<T>(string secretName) where T : class
    {
        var json = await GetSecretAsync(secretName);
        return JsonSerializer.Deserialize<T>(json) 
            ?? throw new InvalidOperationException($"Failed to deserialize {secretName}");
    }
    
    public Task<string> GetConnectionStringAsync()
    {
        return GetSecretAsync("prod/ABPAWSTest/ConnectionString");
    }
}
```

### 6.3 Integration Testing

```csharp
public class SecretsManagerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SecretsManagerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Should_Connect_To_Database_With_Secret()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/health");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

---

## Step 7: Monitoring & Observability

### 7.1 CloudWatch Metrics

```csharp
public class MonitoredSecretsManagerService : ISecretsManagerService
{
    private readonly ISecretsManagerService _inner;
    private readonly IMetrics _metrics;
    private readonly ILogger<MonitoredSecretsManagerService> _logger;

    public async Task<string> GetSecretAsync(string secretName)
    {
        using var activity = Activity.StartActivity("SecretsManager.GetSecret");
        activity?.SetTag("secret.name", secretName);
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var result = await _inner.GetSecretAsync(secretName);
            
            _metrics.Counter("secrets_manager.requests")
                .WithTag("secret_name", secretName)
                .WithTag("status", "success")
                .Increment();
                
            _metrics.Timer("secrets_manager.duration")
                .WithTag("secret_name", secretName)
                .Record(stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            _metrics.Counter("secrets_manager.requests")
                .WithTag("secret_name", secretName)
                .WithTag("status", "error")
                .WithTag("error_type", ex.GetType().Name)
                .Increment();
                
            _logger.LogError(ex, "Failed to retrieve secret {SecretName}", secretName);
            throw;
        }
    }
}
```

### 7.2 Health Checks

```csharp
public class SecretsManagerHealthCheck : IHealthCheck
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly ILogger<SecretsManagerHealthCheck> _logger;

    public SecretsManagerHealthCheck(
        IAmazonSecretsManager secretsManager,
        ILogger<SecretsManagerHealthCheck> logger)
    {
        _secretsManager = secretsManager;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to list secrets to verify connection
            var request = new ListSecretsRequest { MaxResults = 1 };
            await _secretsManager.ListSecretsAsync(request, cancellationToken);
            
            return HealthCheckResult.Healthy("AWS Secrets Manager is accessible");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AWS Secrets Manager health check failed");
            return HealthCheckResult.Unhealthy("AWS Secrets Manager is not accessible", ex);
        }
    }
}

// Register in Program.cs
builder.Services.AddHealthChecks()
    .AddCheck<SecretsManagerHealthCheck>("secrets-manager");
```

---

## Step 8: Advanced Scenarios

### 8.1 Dynamic Configuration Reload

```csharp
public class DynamicSecretsConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly ISecretsManagerService _secretsManager;
    private readonly Timer _reloadTimer;
    private readonly string _secretName;

    public DynamicSecretsConfigurationProvider(
        ISecretsManagerService secretsManager,
        string secretName)
    {
        _secretsManager = secretsManager;
        _secretName = secretName;
        
        // Reload every 5 minutes
        _reloadTimer = new Timer(ReloadSecrets, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    private async void ReloadSecrets(object state)
    {
        try
        {
            var secretValue = await _secretsManager.GetSecretAsync(_secretName);
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(secretValue);
            
            Data.Clear();
            foreach (var kvp in config)
            {
                Data[kvp.Key] = kvp.Value;
            }
            
            OnReload();
        }
        catch (Exception ex)
        {
            // Log error but don't throw to avoid crashing the timer
            Console.WriteLine($"Failed to reload secrets: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _reloadTimer?.Dispose();
    }
}
```

### 8.2 Multi-Region Failover

```csharp
public class MultiRegionSecretsManagerService : ISecretsManagerService
{
    private readonly List<IAmazonSecretsManager> _clients;
    private readonly ILogger<MultiRegionSecretsManagerService> _logger;

    public MultiRegionSecretsManagerService(
        IConfiguration configuration,
        ILogger<MultiRegionSecretsManagerService> logger)
    {
        _logger = logger;
        _clients = new List<IAmazonSecretsManager>();
        
        // Create clients for multiple regions
        var regions = new[] { "us-east-1", "us-west-2", "eu-west-1" };
        foreach (var region in regions)
        {
            var config = new AmazonSecretsManagerConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };
            _clients.Add(new AmazonSecretsManagerClient(config));
        }
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        Exception lastException = null;
        
        foreach (var client in _clients)
        {
            try
            {
                var request = new GetSecretValueRequest { SecretId = secretName };
                var response = await client.GetSecretValueAsync(request);
                
                _logger.LogInformation("Retrieved secret from region {Region}", 
                    client.Config.RegionEndpoint.SystemName);
                
                return response.SecretString;
            }
            catch (Exception ex)
            {
                lastException = ex;
                _logger.LogWarning(ex, "Failed to retrieve secret from region {Region}", 
                    client.Config.RegionEndpoint.SystemName);
            }
        }
        
        throw new InvalidOperationException(
            "Failed to retrieve secret from all regions", lastException);
    }
}
```

---

## Conclusion

AWS Secrets Manager integration with ABP Framework significantly enhances the security of your applications. With this integration:

 **Centralized Secret Management**: All secrets are managed centrally
 **Better Security**: Encryption through KMS and access control through IAM
 **Audit Trail**: Complete recording of who accessed which secret when
 **Automatic Rotation**: Secrets can be rotated automatically
 **High Availability**: AWS high availability guarantee
**Easy Integration**: Native integration with ABP Framework
**Cost Effective**: Pay only for what you use
**Scalable**: Scales with your application needs

With this post, you can securely utilize AWS Secrets Manager in your ABP Framework applications and bid farewell to secret management concerns in production.

### Key Benefits:
- **Developer Productivity**: No hardcoded secrets in config files
- **Operational Excellence**: Automation of rotation and monitoring
- **Security Compliance**: Meet enterprise security requirements
- **Peace of Mind**: Professional-grade secret management

---

## Additional Resources

- [AWS Secrets Manager Documentation](https://docs.aws.amazon.com/secretsmanager/)
- [ABP Framework Documentation](https://docs.abp.io/)
- [AWS SDK for .NET](https://docs.aws.amazon.com/sdk-for-net/)
- [AWS Security Best Practices](https://aws.amazon.com/architecture/security-identity-compliance/)
- [Sample Project Repository](https://github.com/fahrigedik/AWSIntegrationABP)