(function ($) {

    var l = abp.localization.getResource('CmsKit');

    var myDefaultWhiteList = $.fn.tooltip.Constructor.Default.whiteList;

    if (myDefaultWhiteList.img.indexOf('data-name') < 0) {
        myDefaultWhiteList['img'].push('data-name');
    }

    $(document).ready(function () {

        abp.widgets.CmsReactionSelection = function ($widget) {
            var widgetManager = $widget.data('abp-widget-manager');
            var $reactionArea = $widget.find('.cms-reaction-area');
            var $selectIcon = $widget.find('.cms-reaction-select-icon');
            var $popoverContent = $widget.find('.cms-reaction-selection-popover-content');

            function getFilters() {
                return {
                    entityType: $reactionArea.attr('data-entity-type'),
                    entityId: $reactionArea.attr('data-entity-id')
                };
            }

            function registerClickOfReactionIcons($container) {
                $container.find('.cms-reaction-icon').each(function () {
                    var $icon = $(this);
                    $icon.click(function () {
                        var methodName = $icon.hasClass('cms-reaction-icon-selected') ? 'delete' : 'create';
                        volo.cmsKit.reactions.reactionPublic[methodName](
                            $.extend(getFilters(), {
                                reactionName: $icon.attr('data-name')
                            })
                        ).then(function () {
                            $selectIcon.popover('hide');
                            widgetManager.refresh($widget);
                        });
                    });
                });
            }

            function init() {

                $selectIcon.popover({
                    placement: 'right',
                    html: true,
                    title: l('PickYourReaction'),
                    content: $popoverContent.html()
                }).on('shown.bs.popover', function () {
                    var $popover = $('#' + $selectIcon.attr('aria-describedby'));
                    registerClickOfReactionIcons($popover);
                });

                registerClickOfReactionIcons($widget);
            }

            return {
                init: init,
                getFilters: getFilters
            };
        };

        $('.abp-widget-wrapper[data-widget-name="CmsReactionSelection"]')
            .each(function () {
                var widgetManager = new abp.WidgetManager({
                    wrapper: $(this),
                });

                widgetManager.init();
            });
    });
})(jQuery);
