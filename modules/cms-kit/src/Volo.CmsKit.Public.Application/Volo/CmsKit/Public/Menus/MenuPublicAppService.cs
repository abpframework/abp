using System.Threading.Tasks;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Public.Menus
{    
    [RequiresGlobalFeature(typeof(MenuFeature))]
    public class MenuPublicAppService : CmsKitPublicAppServiceBase, IMenuPublicAppService
    {
        protected IMenuRepository MenuRepository { get; }

        public MenuPublicAppService(IMenuRepository menuRepository)
        {
            MenuRepository = menuRepository;
        }
        
        public async Task<MenuWithDetailsDto> GetMainMenuAsync()
        {
            var menu = await MenuRepository.FindMainMenuAsync(includeDetails: true);

            if (menu == null)
            {
                return null;
            }
            
            return ObjectMapper.Map<Menu, MenuWithDetailsDto>(menu);
        }
    }
}