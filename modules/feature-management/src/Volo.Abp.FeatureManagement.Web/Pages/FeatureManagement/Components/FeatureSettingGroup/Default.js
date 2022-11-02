(function($){

    $(function(){
        var _featuresModal = new abp.ModalManager(
            abp.appPath + 'FeatureManagement/FeatureManagementModal'
        );

        $("#ManageHostFeatures").click(function (e) {
            e.preventDefault();
            _featuresModal.open({
                providerName: 'T'
            });
        });
    })
    
})(jQuery);