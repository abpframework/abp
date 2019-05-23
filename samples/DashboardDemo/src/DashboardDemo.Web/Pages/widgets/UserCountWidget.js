(function ($) {

    var $container = $('#UserCountWidgetContainer');

    var _identityUserAppService = volo.abp.identity.identityUser;

    _identityUserAppService.getList({}).then(function(result) {
        $container.find('#UserCount').text(result.items.length);
    });
})(jQuery);
