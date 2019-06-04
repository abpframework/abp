(function ($) {

    var $container = $('#UserCountWidgetContainer');
    if ($container.length > 0) {
        var _identityUserAppService = volo.abp.identity.identityUser;
        _identityUserAppService.getList({}).then(function(result) {
            $container.find('#UserCount').text(result.items.length);
        });
    }
})(jQuery);
