var abp = abp || {};
$(function () {
    abp.modals.updateMenuItem = function () {

        var initModal = function (publicApi, args) {

            var $pageId = $('#ViewModel_PageId');
            var $url = $('#ViewModel_Url');
            var $displayName = $('#ViewModel_DisplayName');
            var $pageIdClearButton = $('#page-id-clear-button');

            initSelectPageId();

            $pageId.on('change', function (params) {
                $url.prop('disabled', $pageId.val());

                if ($pageId.val())
                {
                    $pageIdClearButton.show();
                    if (!$displayName.val()){
                        $displayName.val($pageId.text());
                    }
                }
                else
                {
                    $pageIdClearButton.hide();
                }
            })

            $pageId.trigger('change');
            
            $pageIdClearButton.click(function (){
                $pageId.val('');
                $pageId.trigger('change');
            });
            
            function initSelectPageId() {
                $pageId.data('autocompleteApiUrl', '/api/cms-kit-admin/pages/lookup');
                $pageId.data('autocompleteDisplayProperty', 'title');
                $pageId.data('autocompleteValueProperty', 'id');
                $pageId.data('autocompleteItemsProperty', 'items');
                $pageId.data('autocompleteFilterParamName', 'filter');

                abp.dom.initializers.initializeAutocompleteSelects($pageId);
            }
        };

        return {
            initModal: initModal
        };
    };
});