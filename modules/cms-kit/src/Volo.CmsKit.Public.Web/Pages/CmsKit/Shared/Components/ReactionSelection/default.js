(function () {
    $(document).ready(function () {

        $('.cms-reaction-selection').each(function () {
            var $this = $(this);
            var $selectionButton = $this.find('.cms-reaction-selection-button');
            var $selectionArea = $this.find('.cms-reaction-selection-area');

            $selectionButton.popover({
                html: true,
                placement: 'right',
                content: $selectionArea.html()
            });
        })
    });
})();
