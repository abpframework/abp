(function ($) {
    $(document).ready(function () {

        abp.widgets.CmsReactionSelection= function ($widget) {

            var getFilters = function () {
                return {

                };
            }

            var refresh = function (filters) {

            };

            var init = function (filters) {
                var $wrapper = $widget.find('.cms-reaction-selection');
                $widget.find('.cms-reaction-icon').each(function () {
                    var $icon = $(this);
                    $icon.click(function () {
                        var methodName = $icon.hasClass('cms-reaction-icon-selected') ? 'delete' : 'create';
                        volo.cmsKit.reactions.reactionPublic[methodName]({
                            entityType: $wrapper.attr('data-entity-type'),
                            entityId: $wrapper.attr('data-entity-id'),
                            reactionName: $icon.attr('data-name')
                        }).then(function () {
                            location.reload(); //TODO: JUST TESTING !!!!!!!!
                        });
                    });
                });
            };

            return {
                getFilters: getFilters,
                init: init,
                refresh: refresh
            };
        };

        $('.abp-widget-wrapper[data-widget-name="CmsReactionSelection"]').each(function(){
            var $widget = $(this);
            var widgetManager = new abp.WidgetManager({
                wrapper: $widget.parent(), //TODO: Change to $widget once WidgetManager supports it!
            });

            widgetManager.init();
        });
    });
})(jQuery);
