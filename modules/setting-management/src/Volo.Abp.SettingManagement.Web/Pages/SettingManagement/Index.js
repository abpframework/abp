(function ($) {
    var l = abp.localization.getResource('AbpSettingManagement');

    $('#tabs-nav .nav-item .nav-link').click(function () {
        var _this = $(this);
        if(_this.attr('data-bs-target') !== undefined) {
            return;
        }

        var id = _this.data("id")
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
               _this.attr('data-bs-target', '#' + $.escapeSelector($.escapeSelector(id)));
               $('#tab-content').append('<div id=' + $.escapeSelector(id) + ' class="tab-pane fade active show">' + response + '</div>');
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
