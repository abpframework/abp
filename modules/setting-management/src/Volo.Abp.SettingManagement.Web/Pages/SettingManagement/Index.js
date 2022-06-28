(function ($) {
    var l = abp.localization.getResource('AbpSettingManagement');

    $('#tabs-nav .nav-item').click(function () {
        var id = $(this).attr("id")
        abp.ui.block({
          elm: '#tab-content',
          busy: true,
          promise: abp.ajax({
               type: "POST",
               url: "SettingManagement?handler=RenderView&id=" + id,
               dataType: "html",
               contentType: false,
               processData: false
           }).done(function (response) {
               $('#tab-content').html(response);
               abp.event.trigger('Abp.SettingManagement.View.Render.' + id);
           })
        });
    }).first().click();

    $(document).on('AbpSettingSaved', function () {
        abp.notify.success(l('SuccessfullySaved'));

        abp.ajax({
           url: abp.appPath + 'SettingManagement?handler=RefreshConfiguration'
        });
    });
})(jQuery);
