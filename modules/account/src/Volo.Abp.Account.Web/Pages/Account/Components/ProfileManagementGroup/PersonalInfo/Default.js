(function ($) {
    $(function () {
        var l = abp.localization.getResource("AbpAccount");

        $('#PersonalSettingsForm').submit(function (e) {
            e.preventDefault();

            if (!$('#PersonalSettingsForm').valid()) {
                return false;
            }

            var input = $('#PersonalSettingsForm').serializeFormToObject();

            volo.abp.account.profile.update(input).then(function (result) {
                abp.notify.success(l('PersonalSettingsSaved'));
                updateConcurrencyStamp();
            });
        });
    });

    abp.event.on('passwordChanged', updateConcurrencyStamp);
    
    function updateConcurrencyStamp(){
        volo.abp.account.profile.get().then(function(profile){
            $("#ConcurrencyStamp").val(profile.concurrencyStamp);
        });
    }
})(jQuery);
