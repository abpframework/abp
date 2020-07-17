(function ($) {
    $(document).ready(function () {

        abp.widgets.CmsReactionSelection = function ($widget) {

            function getFilters() {
                return {};
            }

            function refresh(filters) {
                location.reload(); //TODO: JUST TESTING !!!!!!!!
            }

            function init(filters) {
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
                            refresh();
                        });
                    });
                });
            }

            return {
                init: init,
                refresh: refresh,
                getFilters : getFilters
            };
        };

        $('.abp-widget-wrapper[data-widget-name="CmsReactionSelection"]').each(function () {
            var widgetManager = new abp.WidgetManager({
                wrapper: $(this),
            });

            widgetManager.init();
        });
    });
})(jQuery);
