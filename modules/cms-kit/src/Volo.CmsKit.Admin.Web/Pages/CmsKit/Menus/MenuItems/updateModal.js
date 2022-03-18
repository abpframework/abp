var abp = abp || {};
$(function () {
    abp.modals.updateMenuItem = function () {

        var initModal = function (publicApi, args) {

            var $pageId = $('#ViewModel_PageId');
            var $url = $('#ViewModel_Url');
            var $displayName = $('#ViewModel_DisplayName');
            var $pageIdClearButton = $('#url-tab');

            initSelectPageId();

            $pageId.on('change', function (params) {
                $url.prop('disabled', $pageId.val());

                if ($pageId.val()) {
                    if (!$displayName.val()) {
                        $displayName.val($pageId.text());
                    }
                }
            })

            $pageId.trigger('change');

            $pageIdClearButton.click(function () {
                $pageId.val('');
                $pageId.trigger('change');
            });

            function initSelectPageId() {
                $pageId.data('autocompleteApiUrl', '/api/cms-kit-admin/menu-items/lookup/pages');
                $pageId.data('autocompleteDisplayProperty', 'title');
                $pageId.data('autocompleteValueProperty', 'id');
                $pageId.data('autocompleteItemsProperty', 'items');
                $pageId.data('autocompleteFilterParamName', 'filter');
                $pageId.data('autocompleteParentSelector', '#menu-update-modal');

                abp.dom.initializers.initializeAutocompleteSelects($pageId);
            }
        };

        return {
            initModal: initModal
        };
    };
});