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
                    // TODO: Rename checkbox id to something more meaningful.
                    $('#checkbox').prop('checked', response.commentRequireApprovement); // TODO: use $wrapper.find('#checkbox').prop('checked', response.commentRequireApprovement);
                })
            };

            var _bindEvents = function () {
                $('#save').click(function () {
                    var isChecked = $('#checkbox').prop('checked');
                    _service.setSettings({ commentRequireApprovement: isChecked }).then(function (response) {
                        abp.notify.success(l("SavedSuccessfully"));
                    })
                });
            };

        return {
            init: _init
        };
    };
})();
