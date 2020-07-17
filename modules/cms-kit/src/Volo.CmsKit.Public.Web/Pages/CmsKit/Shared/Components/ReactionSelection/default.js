(function ($) {
    $(document).ready(function () {

        abp.widgets.CmsReactionSelection = function ($widget) {
            var $reactionSelection = $widget.find('.cms-reaction-selection');
            var widgetManager = $widget.data('abp-widget-manager');

            function getFilters() {
                return {
                    entityType: $reactionSelection.attr('data-entity-type'),
                    entityId: $reactionSelection.attr('data-entity-id')
                };
            }

            function init(filters) {
                $widget.find('.cms-reaction-icon').each(function () {
                    var $icon = $(this);
                    $icon.click(function () {
                        var methodName = $icon.hasClass('cms-reaction-icon-selected') ? 'delete' : 'create';
                        volo.cmsKit.reactions.reactionPublic[methodName](
                            $.extend(getFilters(), {
                                reactionName: $icon.attr('data-name')
                            })
                        ).then(function () {
                            widgetManager.refresh($widget);
                        });
                    });
                });
            }

            return {
                init: init,
                getFilters: getFilters
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
