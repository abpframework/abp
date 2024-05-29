var l = abp.localization.getResource("CmsKit");
(function () {
    abp.widgets.CmsCommentSetting = function ($wrapper) {
            var _service = volo.cmsKit.admin.comments.commentAdmin
            ;
            var _init = function () {
                _getSettings();
                _bindEvents();
            };
            var _getSettings = function () {
                _service.getSettings().then(function (response) {
                    $wrapper.find('#RequireApprovementCheckbox').prop('checked', response.commentRequireApprovement);
                })
            };

            var _bindEvents = function () {
                $('#save').click(function () {
                    var isChecked = $('#RequireApprovementCheckbox').prop('checked');
                    _service.updateSettings({ commentRequireApprovement: isChecked }).then(function (response) {
                        abp.notify.success(l("SavedSuccessfully"));
                    })
                });
            };

        return {
            init: _init
        };
    };
})();
