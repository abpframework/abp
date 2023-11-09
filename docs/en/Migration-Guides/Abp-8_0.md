# ABP Version 8.0 Migration Guide

This document is a guide for upgrading ABP v7.4.x solutions to ABP v8.0.x.

## Upgraded NuGet Dependencies

The following NuGet libraries have been upgraded:

| Package             | Old Version | New Version |
| ------------------- | ----------- | ----------- |
| All Microsoft packages | 7.x       | 8.x       |
| Microsoft.CodeAnalysis | 4.2.0       | 4.5.0      |
| NUglify | 1.20.0       | 1.21.0       |
| Polly | 7.2.3       | 8.0.0       |
| aliyun-net-sdk-sts | 3.1.0       | 3.1.1       |
| Autofac | 7.0.0       | 7.1.0       |
| Autofac.Extras.DynamicProxy | 6.0.1       | 7.1.0       |
| AutoMapper | 12.0.0       | 12.0.1       |
| AsyncKeyedLock | 6.2.1       | 6.2.2       |
| AWSSDK.S3 | 3.7.9.2       | 3.7.205.9       | 
| AWSSDK.SecurityToken | 3.7.1.151       | 3.7.202.4       |
| Azure.Storage.Blobs | 12.15.0       | 12.18.0       |
| ConfigureAwait.Fody | 3.3.1       | 3.3.2       |
| Confluent.Kafka | 1.8.2       | 2.2.0       |
| Dapper | 2.0.123       | 2.1.4       |
| Dapr.Client | 1.11.0       | 1.9.0       |
| DistributedLock.Core | 1.0.4       | 1.0.5       |
| DistributedLock.Redis | 1.0.1       | 1.0.2       |
| EphemeralMongo.Core | 1.1.0       | 1.1.3       |
| EphemeralMongo6.runtime.linux-x64 | 1.1.0       | 1.1.3       |
| EphemeralMongo6.runtime.osx-x64 | 1.1.0       | 1.1.3       |
| EphemeralMongo6.runtime.win-x64 | 1.1.0       | 1.1.3       |
| FluentValidation | 11.0.1       | 11.7.1       |
| Fody | 6.6.1       | 6.8.0       |
| Hangfire.AspNetCore | 1.8.2       | 1.8.5       |
| Hangfire.SqlServer | 1.8.2       | 1.8.5       |
| HtmlSanitizer | 5.0.331       | 8.0.723       |
| IdentityModel | 6.0.0       | 6.2.0       |
| IdentityServer4.AspNetIdentity | 4.1.1       | 4.1.2       |
| JetBrains.Annotations | 2022.1.0       | 2023.2.0       |
| LibGit2Sharp | 0.26.2       | 0.27.2       |
| Magick.NET-Q16-AnyCPU | 13.2.0       | 13.3.0       |
| MailKit | 3.2.0       | 4.2.0       |
| Markdig.Signed | 0.26.0       | 0.33.0       |
| Microsoft.AspNetCore.Mvc.Versioning | 5.0.0       | 5.1.0       |
| Microsoft.AspNetCore.Razor.Language | 6.0.8       | 6.0.22       |
| Microsoft.NET.Test.Sdk | 17.2.0       | 17.7.2       |
| Minio | 4.0.6       | 6.0.0       |
| MongoDB.Driver | 2.19.1       | 2.21.0       |
| NEST | 7.14.1       | 7.17.5       |
| Newtonsoft.Json | 13.0.1       | 13.0.3       |
| NSubstitute | 4.3.0       | 5.1.0       |
| NSubstitute.Analyzers.CSharp | 1.0.15       | 1.0.16       |
| NuGet.Versioning | 5.11.0       | 6.7.0       |
| Octokit | 0.50.0       | 8.0.1       |
| Quartz | 3.4.0       | 3.7.0       |
| Quartz.Extensions.DependencyInjection | 3.4.0       | 3.7.0       |
| Quartz.Plugins.TimeZoneConverter | 3.4.0       | 3.7.0       |
| RabbitMQ.Client | 6.3.0       | 6.5.0       |
| Rebus | 6.6.5       | 7.2.1       |
| Rebus.ServiceProvider | 7.0.0       | 9.1.0       |
| Scriban | 5.4.4       | 5.9.0       |
| Serilog.AspNetCore | 5.0.0       | 7.0.0       |
| Serilog.Extensions.Hosting | 3.1.0       | 7.0.0       |
| Serilog.Extensions.Logging | 3.1.0       | 7.0.0       |
| Serilog.Sinks.Async | 1.4.0       | 1.5.0       |
| SharpZipLib | 1.3.3       | 1.4.2       |
| Shouldly | 4.1.0       | 4.2.1       |
| SixLabors.ImageSharp | 1.0.4       | 3.0.2       |
| Slugify.Core | 3.0.0       | 4.0.1       |
| Spectre.Console | 0.46.1-preview.0.7       | 0.47.0       |
| Swashbuckle.AspNetCore | 6.2.1       | 6.5.0       |
| System.Linq.Dynamic.Core | 1.3.3       | 1.3.5       |
| TimeZoneConverter | 5.0.0       | 6.1.0       |
| xunit | 2.4.1       | 2.5.1       |
| xunit.extensibility.execution | 2.4.1       | 2.5.1       |
| xunit.runner.visualstudio | 2.4.5       | 2.5.1       |
