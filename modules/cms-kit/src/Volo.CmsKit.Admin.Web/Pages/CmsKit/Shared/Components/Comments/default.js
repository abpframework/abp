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
            var $requireApprovalCheckbox = $('#RequireApprovementCheckbox');
            var previousValue = $requireApprovalCheckbox.prop('checked');

            $wrapper.find('#Save').click(function () {
                var isRequireApproved = $requireApprovalCheckbox.prop('checked');

                function UpdateSettings(commentRequireApprovement) {
                    _service.updateSettings({commentRequireApprovement: commentRequireApprovement}).then(function (response) {
                        abp.notify.success(l("SavedSuccessfully"));
                        previousValue = commentRequireApprovement;
                    })
                }

                UpdateSettings(isRequireApproved);
            });
        };

        return {
            init: _init
        };
    };
})();
