$(function (){
    var l = abp.localization.getResource("CmsKit");
    
    var service = volo.cmsKit.admin.globalResources.globalResourceAdmin;

    $('#SaveResourcesButton').on('click','',function(){
        service.setGlobalResources(
            {
                style: $('#StyleContent').val(),
                script: $('#ScriptContent').val()
            }
        ).then(function () {
            abp.message.success(l("SavedSuccessfully"));
        });
    });
});