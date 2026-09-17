var abp = abp || {};
$(function () {
    abp.modals.projectGeneratePdf = function () {

        var l = abp.localization.getResource('Docs');
        var projectAppService = volo.docs.projects.docsProject;
        
        var initModal = function (publicApi, args) {
            
        };

        $("#Versions").change(function () {
            var shortname = $("#ShortName").val();
            var version = $("#Version").val();
            projectAppService.getLanguageList(shortname, version).done(function (result) {
                $("#Language").empty();
                $.each(result.languages, function (index, item) {
                    $("#Language").append($(`<option value="${item.code}">${item.displayName}</option>`));
                });
            });
        })
        
        return {
            initModal: initModal,
        };
    };
});
