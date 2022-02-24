$(function (){
    var l = abp.localization.getResource("CmsKit");
    
    var service = volo.cmsKit.admin.globalResources.globalResourceAdmin;

    $('#SaveResourcesButton').on('click','',function(){
        service.setGlobalStyle($('#StyleContent').val()).then(function () {
            service.setGlobalScript($('#ScriptContent').val()).then(function () {
                abp.message.success(l("SavedSuccessfully"));
            });
        });
    });
});