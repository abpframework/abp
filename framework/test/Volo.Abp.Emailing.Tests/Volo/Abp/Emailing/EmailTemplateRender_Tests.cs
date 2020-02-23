using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Emailing
{
    public class EmailTemplateRender_Tests : AbpIntegratedTest<AbpEmailingTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        private readonly ITemplateRender _templateRender;

        public EmailTemplateRender_Tests()
        {
            _templateRender = GetRequiredService<ITemplateRender>();
        }

        [Fact]
        public async Task RenderAsync()
        {
            var template = "Hello {{email}} {{ for order in orders }}{{ order.id }}:{{ order.name }},{{ end }}";

            var model = new ModelClass
            {
                Email = "john@abp.io",
                Orders = new List<ModelClass.Order>
                {
                    new ModelClass.Order
                    {
                        Id = "1",
                        Name = "iphone"
                    },
                    new ModelClass.Order
                    {
                        Id = "2",
                        Name = "ipad"
                    }
                }
            };

            var result = await _templateRender.RenderAsync(template, model);
            result.ShouldBe("Hello john@abp.io 1:iphone,2:ipad,");
        }

        public class ModelClass
        {
            public string Email { get; set; }

            public List<Order> Orders { get; set; }

            public class Order
            {
                public string Id { get; set; }

                public string Name { get; set; }
            }
        }
    }
}