(function ($) {
    var l = abp.localization.getResource('AbpSettingManagement');

    $('#tabs-nav .nav-item .nav-link').click(function () {
        var _this = $(this);
        if(_this.attr('data-bs-target') !== undefined) {
            return;
        }

        var id = _this.data("id");
        var tabId = id.replace(/\./g, '-');
        abp.ui.block({
          elm: '#tab-content',
          busy: true,
          promise: abp.ajax({
               type: 'POST',
               url: 'SettingManagement?handler=RenderView&id=' + id,
               dataType: "html",
               contentType: false,
               processData: false
           }).done(function (response) {
               $('#tab-content').children('.tab-pane').removeClass('show').removeClass('active');
               _this.attr('data-bs-target', '#' + tabId);
               $('#tab-content').append('<div id=' + tabId + ' class="tab-pane fade active show abp-md-form">' + response + '</div>');
           })
        });
    }).first().click();

    $(document).on('AbpSettingSaved', function () {
        abp.notify.success(l('SavedSuccessfully'));

        abp.ajax({
           url: abp.appPath + 'SettingManagement?handler=RefreshConfiguration'
        });
    });
})(jQuery);
