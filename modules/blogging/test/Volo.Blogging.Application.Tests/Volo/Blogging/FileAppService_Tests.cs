using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Validation;
using Volo.Blogging.Files;
using Xunit;

namespace Volo.Blogging
{
    public class FileAppService_Tests : BloggingApplicationTestBase
    {
        private readonly IFileAppService _fileAppService;
        private IBlobContainer<BloggingFileContainer> _blobContainer;
        private FakeAuthorizationService _authorizationService;

        public FileAppService_Tests()
        {
            _fileAppService = GetRequiredService<IFileAppService>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _blobContainer = Substitute.For<IBlobContainer<BloggingFileContainer>>();
            _blobContainer.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(_ => new MemoryStream(new byte[] { 1, 2, 3 }));
            services.AddSingleton(_blobContainer);

            _authorizationService = new FakeAuthorizationService();
            services.Replace(ServiceDescriptor.Singleton<IAuthorizationService>(_authorizationService));
            services.Replace(ServiceDescriptor.Singleton<IAbpAuthorizationService>(_authorizationService));
        }

        [Theory]
        [InlineData("my-image.png")]
        [InlineData("my..image.png")]
        public async Task Should_Get_File(string name)
        {
            (await _fileAppService.GetAsync(name)).Bytes.ShouldBe(new byte[] { 1, 2, 3 });
            (await _fileAppService.GetFileAsync(name)).FileName.ShouldBe(name);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(".")]
        [InlineData("..")]
        [InlineData("../my-image.png")]
        [InlineData("..\\my-image.png")]
        [InlineData("my-folder/my-image.png")]
        [InlineData("my-folder\\my-image.png")]
        public async Task Should_Not_Get_File_With_Invalid_Name(string name)
        {
            await Should.ThrowAsync<AbpValidationException>(() => _fileAppService.GetAsync(name));
            await Should.ThrowAsync<AbpValidationException>(() => _fileAppService.GetFileAsync(name));

            await _blobContainer.DidNotReceiveWithAnyArgs().GetAsync(default, default);
        }

        [Theory]
        [InlineData(BloggingPermissions.Posts.Create)]
        [InlineData(BloggingPermissions.Posts.Update)]
        public async Task Should_Upload_Image_With_Create_Or_Update_Permission(string permission)
        {
            _authorizationService.GrantedPolicies.Add(permission);

            var output = await _fileAppService.CreateAsync(CreateUploadInput());

            output.Name.ShouldEndWith(".png");
            output.WebUrl.ShouldBe("/api/blogging/files/www/" + output.Name);
            await _blobContainer.Received(1).SaveAsync(output.Name, Arg.Any<Stream>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Should_Not_Upload_Image_Without_Permission()
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Delete);

            await Should.ThrowAsync<AbpAuthorizationException>(() => _fileAppService.CreateAsync(CreateUploadInput()));

            await _blobContainer.DidNotReceiveWithAnyArgs().SaveAsync(default, default(Stream), default, default);
        }

        private static FileUploadInputDto CreateUploadInput()
        {
            var stream = new MemoryStream();
            using (var image = new Image<Rgba32>(1, 1))
            {
                image.SaveAsPng(stream);
            }

            stream.Position = 0;

            return new FileUploadInputDto
            {
                Name = "my-image.png",
                File = new RemoteStreamContent(stream, "my-image.png", "image/png")
            };
        }

        private class FakeAuthorizationService : IAbpAuthorizationService
        {
            public HashSet<string> GrantedPolicies { get; } = new HashSet<string>();

            public IServiceProvider ServiceProvider => null;

            public ClaimsPrincipal CurrentPrincipal { get; } = new ClaimsPrincipal();

            public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
            {
                return Task.FromResult(AuthorizationResult.Success());
            }

            public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
            {
                return Task.FromResult(GrantedPolicies.Contains(policyName) ? AuthorizationResult.Success() : AuthorizationResult.Failed());
            }
        }
    }
}
