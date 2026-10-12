using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using SkiaSharp;
using Volo.Abp;
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
        private const string GifImageBase64 = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        private const string BmpImageBase64 = "Qk06AAAAAAAAADYAAAAoAAAAAQAAAAEAAAABABgAAAAAAAQAAAATCwAAEwsAAAAAAAAAAAAA/wAAAA==";

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

            byte[] savedBytes = null;
            _blobContainer
                .When(x => x.SaveAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()))
                .Do(callInfo =>
                {
                    using var memoryStream = new MemoryStream();
                    callInfo.ArgAt<Stream>(1).CopyTo(memoryStream);
                    savedBytes = memoryStream.ToArray();
                });

            var imageBytes = CreateImage(SKEncodedImageFormat.Png);
            var output = await _fileAppService.CreateAsync(CreateUploadInput(imageBytes));

            output.Name.ShouldEndWith(".png");
            output.WebUrl.ShouldBe("/api/blogging/files/www/" + output.Name);
            await _blobContainer.Received(1).SaveAsync(output.Name, Arg.Any<Stream>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());
            savedBytes.ShouldBe(imageBytes);
        }

        [Fact]
        public async Task Should_Restore_Stream_Position_Before_Saving()
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Create);

            long? positionOnSave = null;
            _blobContainer
                .When(x => x.SaveAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()))
                .Do(callInfo => positionOnSave = callInfo.ArgAt<Stream>(1).Position);

            using var stream = new MemoryStream();
            stream.Write(new byte[16]);
            stream.Write(CreateImage(SKEncodedImageFormat.Png));
            stream.Position = 16;

            await _fileAppService.CreateAsync(new FileUploadInputDto
            {
                Name = "my-image.png",
                File = new RemoteStreamContent(stream, "my-image.png", "image/png")
            });

            positionOnSave.ShouldBe(16L);
        }

        [Theory]
        [InlineData(SKEncodedImageFormat.Jpeg)]
        [InlineData(SKEncodedImageFormat.Png)]
        public async Task Should_Upload_Allowed_Image_Formats(SKEncodedImageFormat format)
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Create);

            await _fileAppService.CreateAsync(CreateUploadInput(CreateImage(format)));

            await _blobContainer.ReceivedWithAnyArgs(1).SaveAsync(default, default(Stream), default, default);
        }

        [Theory]
        [InlineData(GifImageBase64)]
        [InlineData(BmpImageBase64)]
        public async Task Should_Upload_Gif_And_Bmp_Images(string imageBase64)
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Create);

            await _fileAppService.CreateAsync(CreateUploadInput(Convert.FromBase64String(imageBase64)));

            await _blobContainer.ReceivedWithAnyArgs(1).SaveAsync(default, default(Stream), default, default);
        }

        [Fact]
        public async Task Should_Not_Upload_Image_With_Not_Allowed_Format()
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Create);

            await Should.ThrowAsync<UserFriendlyException>(() => _fileAppService.CreateAsync(CreateUploadInput(CreateImage(SKEncodedImageFormat.Webp))));

            await _blobContainer.DidNotReceiveWithAnyArgs().SaveAsync(default, default(Stream), default, default);
        }

        [Fact]
        public async Task Should_Not_Upload_Non_Image_File()
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Create);

            var input = CreateUploadInput(Encoding.UTF8.GetBytes("This is not an image"));

            await Should.ThrowAsync<UserFriendlyException>(() => _fileAppService.CreateAsync(input));

            await _blobContainer.DidNotReceiveWithAnyArgs().SaveAsync(default, default(Stream), default, default);
        }

        [Fact]
        public async Task Should_Not_Upload_Image_Without_Permission()
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Delete);

            await Should.ThrowAsync<AbpAuthorizationException>(() => _fileAppService.CreateAsync(CreateUploadInput()));

            await _blobContainer.DidNotReceiveWithAnyArgs().SaveAsync(default, default(Stream), default, default);
        }

        [Fact]
        public async Task Should_Not_Upload_Image_With_Invalid_Name()
        {
            _authorizationService.GrantedPolicies.Add(BloggingPermissions.Posts.Create);

            var input = CreateUploadInput();
            input.Name = "my-image.png\\my-folder";

            await Should.ThrowAsync<AbpValidationException>(() => _fileAppService.CreateAsync(input));

            await _blobContainer.DidNotReceiveWithAnyArgs().SaveAsync(default, default(Stream), default, default);
        }

        private static FileUploadInputDto CreateUploadInput()
        {
            return CreateUploadInput(CreateImage(SKEncodedImageFormat.Png));
        }

        private static FileUploadInputDto CreateUploadInput(byte[] content)
        {
            var stream = new MemoryStream(content);

            return new FileUploadInputDto
            {
                Name = "my-image.png",
                File = new RemoteStreamContent(stream, "my-image.png", "image/png")
            };
        }

        private static byte[] CreateImage(SKEncodedImageFormat format)
        {
            using (var bitmap = new SKBitmap(1, 1))
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(format, 100))
            {
                return data.ToArray();
            }
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
