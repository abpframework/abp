var l = abp.localization.getResource("CmsKit");
(function () {
    abp.widgets.CmsCommentSetting = function ($wrapper) {
        var _service = volo.cmsKit.admin.comments.commentAdmin;

        var _init = function () {
            _getSettings();
            _bindEvents();
        };
        
        var _getSettings = function () {
            $wrapper.find('#RequireApprovementCheckbox').prop('checked', abp.setting.getBoolean("CmsKit.Comments.RequireApprovement"));
        };

        var _bindEvents = function () {
            $wrapper.find('#Save').click(function () {
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
