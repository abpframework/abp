using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Acme.BookStore
{
    public class BookStoreWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<BookStoreWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}