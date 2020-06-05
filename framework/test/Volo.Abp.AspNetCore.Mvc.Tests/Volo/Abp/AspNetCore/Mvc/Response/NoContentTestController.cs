using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Response
{
    [Route("api/NoContent-Test")]
    public class NoContentTestController : AbpController
    {
        [HttpGet]
        [Route("TestMethod")]
        public void TestMethod()
        {
        }

        [HttpGet]
        [Route("TestMethodWithReturn")]
        public string TestMethodWithReturn()
        {
            return "TestReturn";
        }

        [HttpGet]
        [Route("TestCustomHttpStatusCodeMethod")]
        public void TestCustomHttpStatusCodeMethod()
        {
            Response.Redirect("/");
        }

        [HttpGet]
        [Route("TestAsyncMethod")]
        public async Task TestAsyncMethod()
        {
            await Task.CompletedTask;
        }

        [HttpGet]
        [Route("TestAsyncMethodWithReturn")]
        public async Task<string> TestAsyncMethodWithReturn()
        {
            return await Task.FromResult("TestReturn");
        }

        [HttpGet]
        [Route("TestAsyncCustomHttpStatusCodeMethod")]
        public async Task TestAsyncCustomHttpStatusCodeMethod()
        {
            Response.Redirect("/");
            await Task.CompletedTask;
        }
    }
}