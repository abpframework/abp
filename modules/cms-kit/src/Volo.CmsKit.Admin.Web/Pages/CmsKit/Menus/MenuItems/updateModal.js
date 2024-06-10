var abp = abp || {};
$(function () {
    abp.modals.updateMenuItem = function () {

        var initModal = function (publicApi, args) {

            var $pageId = $('#ViewModel_PageId');
            var $url = $('#ViewModel_Url');
            var $displayName = $('#ViewModel_DisplayName');
            var $menuItemForm = $('#menu-item-form');

            $pageId.on('change', function (params) {
                $url.prop('disabled', $pageId.val());

                if ($pageId.val()) {
                    if (!$displayName.val()) {
                        $displayName.val($pageId.text());
                    }
                }
            })

            $pageId.trigger('change');

            $menuItemForm.on('submit', function (e) {
                $('[href="#url"]').tab('show');
            });
        };

        return {
            initModal: initModal
        };
    };
});