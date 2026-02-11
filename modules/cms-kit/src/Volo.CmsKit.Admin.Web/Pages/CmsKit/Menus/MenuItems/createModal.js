var abp = abp || {};
$(function () {
    abp.modals.createMenuItem = function () {

        var initModal = function (publicApi, args) {

            var $pageId = $('#ViewModel_PageId');
            var $url = $('#ViewModel_Url');
            var $displayName = $('#ViewModel_DisplayName');
            var $menuItemForm = $('#menu-item-form');
            var $selectRequiredPermission = $('#requiredPermissionName');

            $pageId.on('change', function (params) {
                $url.prop('disabled', $pageId.val());
                
                if ($pageId.val())
                {
                    if (!$displayName.val()){
                        $displayName.val($pageId.text().trim());
                    }
                }
            })

            function initSelectRequiredPermission(){
                function formatDisplayName(item) {
                    if (!item.id) {
                        return item.text;
                    }
                    var $displayName = $(`<span data-bs-toggle="tooltip" data-bs-container="#tooltip_container" data-bs-placement="top" title="${item.displayName}">${item.id}</span>`);
                    $displayName.tooltip();
                    return $displayName;
                }
                
                $selectRequiredPermission.select2({
                    ajax:{
                        url: '/api/cms-kit-admin/menu-items/lookup/permissions',
                        delay: 250,
                        dataType: "json",
                        data: function (params) {
                            let query = {};
                            query["filter"] = params.term;
                            return query;
                        },
                        processResults: function (data) {
                            let retVal = [];
                            let items = data["items"];
                            $('body').tooltip('dispose');
                            items.forEach(function (item, index) {
                                retVal.push({
                                    id: item["name"],
                                    text: item["name"],
                                    displayName: item["displayName"]
                                })
                            });
                            return {
                                results: retVal
                            };
                        }
                    },
                    templateResult: formatDisplayName,
                    width: '100%',
                    dropdownParent: $('#menu-create-modal'),
                    language: abp.localization.currentCulture.cultureName
                });
            }

            initSelectRequiredPermission();
            
            $menuItemForm.on('submit', function (e) {
                $('[href="#url"]').tab('show');
            });
        };

        return {
            initModal: initModal
        };
    };
});