(function () {
    $(document).ready(function () {

        function initReactionSelection() {
            var $wrapper = $(this);
            var $availableReactions = $wrapper.find('.cms-reaction-selection-available-reactions');

            $wrapper.find('.cms-reaction-icon').each(function () {
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
        }

        $('.cms-reaction-selection').each(initReactionSelection);
    });
})();
