(function ($) {
    var l = abp.localization.getResource('CmsKit');

    abp.widgets.CmsMarkedItemToggle = function ($widget) {

        let widgetManager = $widget.data('abp-widget-manager');
        let $markedItemArea = $widget.find('.cms-markedItem-area');
        let loginModal = new abp.ModalManager(abp.appPath + 'CmsKit/Shared/Modals/Login/LoginModal');

        function getFilters() {
            return {
                entityType: $markedItemArea.attr('data-entity-type'),
                entityId: $markedItemArea.attr('data-entity-id'),
                needsConfirmation: $markedItemArea.attr('data-needs-confirmation')
            };
        }

        function setIconStyles($icon) {
            var iconColor = $icon.css('color');
            $icon.css({
                '-webkit-text-stroke-color': iconColor,
                '-webkit-text-stroke-width': '2px'
            });

            // Manually set the important rule for color
            $icon[0].style.setProperty('color', 'transparent', 'important');
        }

        function isDoubleClicked(element) {
            if (element.data("isclicked")) return true;

            element.data("isclicked", true);
            setTimeout(function () {
                element.removeData("isclicked");
            }, 500);
        }

        function handleUnauthenticated() {
            loginModal.open({
                message: l("CmsKit:MarkedItem:LoginMessage"),
                returnUrl: $markedItemArea.attr('data-return-url')
            });
        }

        function registerClickOfMarkedItemIcon($container) {
            var $icon = $container.find('.cms-markedItem-icon');

            if (isDoubleClicked($icon)) return;
            
            $icon.click(async function () {
                if ($icon.attr('data-is-authenticated') === 'false') {
                    handleUnauthenticated();
                    return;
                }

                if ($icon.hasClass('confirm') && !$icon.hasClass('unmarked')) {
                    const confirmed = await abp.message.confirm(l('CmsKit:MarkedItem:ToggleConfirmation'));
                    if (confirmed) {
                        toggleIcon($icon);
                    }
                    return;
                }
                else {
                    toggleIcon();
                }
            });
        }

        async function toggleIcon() {
            await volo.cmsKit.public.markedItems.markedItemPublic.toggle(
                $markedItemArea.attr('data-entity-type'),
                $markedItemArea.attr('data-entity-id')
            );
           widgetManager.refresh($widget);
        }

        function init() {
            var $unmarked = $widget.find('.unmarked');
            if ($unmarked.length === 1) {
                setIconStyles($unmarked);
            }
            registerClickOfMarkedItemIcon($widget);
        }

        return {
            init: init,
            getFilters: getFilters
        };
    };

})(jQuery);
