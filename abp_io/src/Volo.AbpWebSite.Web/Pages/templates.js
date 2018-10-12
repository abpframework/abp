(function($) {
    $(function() {
        $('#ProjectType').change(function() {
            if ($(this).val() === 'MvcApp') {
                $('#DatabaseProviderFormGroup').show('fast');
            } else {
                $('#DatabaseProviderFormGroup').hide('fast');
            }
        });
    });
})(jQuery);