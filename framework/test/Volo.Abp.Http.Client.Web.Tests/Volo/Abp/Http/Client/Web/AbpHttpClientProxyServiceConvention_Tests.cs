using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.Client.Web;

public class AbpHttpClientProxyServiceConvention_Tests : AbpHttpClientWebTestBase
{
    [Fact]
    public async Task Should_Read_Primitive_Body_Parameter_From_Request_Body()
    {
        var response = await Client.PostAsync(
            "/api/client-proxy-test/from-body",
            new StringContent("\"Server=localhost;Database=Test\"", Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("Server=localhost;Database=Test");
    }

    [Fact]
    public async Task Should_Keep_Reading_Other_Primitive_Parameters_From_Query_String()
    {
        var response = await Client.PostAsync("/api/client-proxy-test/from-query?value=test", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test");
    }
}
