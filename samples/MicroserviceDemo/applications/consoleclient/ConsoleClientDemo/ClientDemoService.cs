using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace ConsoleClientDemo
{
    public class ClientDemoService : ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;

        public ClientDemoService(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task RunAsync()
        {
            await TestIdentityService();
        }

        private async Task TestIdentityService()
        {
            var output = await _userAppService.GetListAsync(new GetIdentityUsersInput());

            Console.WriteLine("*** IdentityService ***");
            Console.WriteLine("Total user count: " + output.TotalCount);

            foreach (var user in output.Items)
            {
                Console.WriteLine($"- UserName={user.UserName}, Email={user.Email}, Name={user.Name}, Surname={user.Surname}");
            }
        }
    }
}