(function () {
    $(document).ready(function () {

        function initReactionSelection(){
            var $this = $(this);
            var $availableReactions = $this.find('.cms-reaction-selection-available-reactions');

            $availableReactions.find('.cms-reaction-icon').each(function(){
                var $icon = $(this);
                $icon.click(function(){
                    volo.cmsKit.reactions.reactionPublic.create({
                        entityType: $this.attr('data-entity-type'),
                        entityId: $this.attr('data-entity-id'),
                        reactionName: $icon.attr('data-name')
                    });
                });
            });
        }

        $('.cms-reaction-selection').each(initReactionSelection);

    });
})();
