(function () {
    $(document).ready(function () {

        function initReactionSelection(){
            var $this = $(this);
            var $availableReactions = $this.find('.cms-reaction-selection-available-reactions');

            $availableReactions.find('.cms-reaction-icon').each(function(){
                var $icon = $(this);
                $icon.click(function(){

                });
            });
        }

        $('.cms-reaction-selection').each(initReactionSelection)

    });
})();
