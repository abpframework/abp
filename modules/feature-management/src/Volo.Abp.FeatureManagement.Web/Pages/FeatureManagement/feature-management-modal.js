var abp = abp || {};
(function ($) {
    abp.modals = abp.modals || {};

    abp.modals.FeatureManagement = function () {

        $('.FeatureValueCheckbox').change(function () {
            if (this.checked) {
                $(this).val("true");
            }
            else {
                $(this).val("false");
            }
        });

    };
})(jQuery);