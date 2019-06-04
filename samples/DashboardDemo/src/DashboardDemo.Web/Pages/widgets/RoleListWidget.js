(function ($) {
    var $container = $('#RoleListWidgetContainer');
    if ($container.length > 0) {
        var _identityUserAppService = volo.abp.identity.identityRole;

        _identityUserAppService.getList({}).then(function (result) {
            var html = '';
            for (var i = 0; i < result.items.length; i++) {
                html += '<li>' + result.items[i].name + '</li>';
            }

            $container.find('#RoleList').html(html);
        });
    }
})(jQuery);
