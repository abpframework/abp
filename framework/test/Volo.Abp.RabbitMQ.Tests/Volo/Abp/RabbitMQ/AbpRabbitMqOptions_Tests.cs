using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shouldly;
using Xunit;

namespace Volo.Abp.RabbitMQ;

public class AbpRabbitMqOptions_Tests
{
    [Fact]
    public void Should_Bind_Connection_Settings_From_Json()
    {
        var connection = GetConnection(
            """
            {
              "RabbitMQ": {
                "Connections": {
                  "Default": {
                    "HostName": "123.123.123.123",
                    "Port": 5672,
                    "MaxInboundMessageBodySize": 500000000
                  }
                }
              }
            }
            """);

        connection.HostName.ShouldBe("123.123.123.123");
        connection.Port.ShouldBe(5672);
        connection.MaxInboundMessageBodySize.ShouldBe(500000000u);
    }

    [Fact]
    public void Should_Bind_Ssl_Settings_From_Json()
    {
        var connection = GetConnection(
            """
            {
              "RabbitMQ": {
                "Connections": {
                  "Default": {
                    "HostName": "rabbit.example.test",
                    "Ssl": {
                      "AcceptablePolicyErrors": "RemoteCertificateChainErrors",
                      "CertPassphrase": "secret",
                      "CheckCertificateRevocation": true,
                      "Enabled": true,
                      "ServerName": "tls.example.test",
                      "Version": "Tls12"
                    }
                  }
                }
              }
            }
            """);

        connection.Ssl.AcceptablePolicyErrors.ShouldBe(SslPolicyErrors.RemoteCertificateChainErrors);
        connection.Ssl.CertPassphrase.ShouldBe("secret");
        connection.Ssl.CheckCertificateRevocation.ShouldBeTrue();
        connection.Ssl.Enabled.ShouldBeTrue();
        connection.Ssl.ServerName.ShouldBe("tls.example.test");
        connection.Ssl.Version.ShouldBe(SslProtocols.Tls12);
    }

    [Fact]
    public void Should_Combine_Uri_With_Advanced_Json_Connection_Settings()
    {
        var connection = GetConnection(
            """
            {
              "RabbitMQ": {
                "Connections": {
                  "Default": {
                    "Uri": "amqps://configured-user:configured-pass@uri.example.test:5678/configured-vhost",
                    "MaxInboundMessageBodySize": 500000000,
                    "Ssl": {
                      "AcceptablePolicyErrors": "RemoteCertificateChainErrors",
                      "CertPassphrase": "secret",
                      "CheckCertificateRevocation": true,
                      "Enabled": true,
                      "ServerName": "tls.example.test",
                      "Version": "Tls12"
                    }
                  }
                }
              }
            }
            """);

        connection.HostName.ShouldBe("uri.example.test");
        connection.Port.ShouldBe(5678);
        connection.UserName.ShouldBe("configured-user");
        connection.Password.ShouldBe("configured-pass");
        connection.VirtualHost.ShouldBe("configured-vhost");
        connection.MaxInboundMessageBodySize.ShouldBe(500000000u);
        connection.Ssl.AcceptablePolicyErrors.ShouldBe(SslPolicyErrors.RemoteCertificateChainErrors);
        connection.Ssl.CertPassphrase.ShouldBe("secret");
        connection.Ssl.CheckCertificateRevocation.ShouldBeTrue();
        connection.Ssl.Enabled.ShouldBeTrue();
        connection.Ssl.ServerName.ShouldBe("tls.example.test");
        connection.Ssl.Version.ShouldBe(SslProtocols.Tls12);
    }

    private static ConnectionFactory GetConnection(
        string json,
        string connectionName = RabbitMqConnections.DefaultConnectionName)
    {
        using var application = AbpApplicationFactory.Create<AbpRabbitMqModule>(options =>
        {
            options.Services.ReplaceConfiguration(
                new ConfigurationBuilder()
                    .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)))
                    .Build());
        });

        application.Initialize();

        return application.ServiceProvider
            .GetRequiredService<IOptions<AbpRabbitMqOptions>>()
            .Value
            .Connections[connectionName];
    }
}
