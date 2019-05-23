(function ($) {
    var $container = $('#RoleListWidgetContainer');

    var _identityUserAppService = volo.abp.identity.identityRole;

    _identityUserAppService.getList({}).then(function (result) {
        var html = '';
        for (var i = 0; i < result.items.length; i++) {
            html += '<li>'+ result.items[i].name+'</li>';
        }

        $container.find('#RoleList').html(html);
    });
})(jQuery);
