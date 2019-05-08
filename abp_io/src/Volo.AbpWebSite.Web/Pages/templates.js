(function($) {
    $(function() {
        $('#ProjectType').change(function() {
            if ($(this).val() === 'MvcApp') {
                $('#DatabaseProviderFormGroup').show('fast');
            } else {
                $('#DatabaseProviderFormGroup').hide('fast');
            }
        });

        $("form").submit(function (event) {
            var regex = $("#ProjectRegex").val();
            var patt = new RegExp(regex);
            var res = patt.test($("#CompanyAndProjectName").val());
            if (!res) {
                abp.message.error("Invalid Project Name.","");
                event.preventDefault();
            }
        });
    });
})(jQuery);