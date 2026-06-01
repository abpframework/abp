var abp = abp || {};
(function ($) {
    var $all = $("#grantAllresourcePermissions");
    var $items = $("#permissionList input[type='checkbox']").not("#grantAllresourcePermissions");
    $all.on("change", function () {
        $items.prop("checked", this.checked);
    });
    $items.on("change", function () {
        $all.prop("checked", $items.length === $items.filter(":checked").length);
    });

    var $permissionManagementForm = $("#addResourcePermissionManagementForm");
    var $providerKey = $("#AddModel_ProviderKey");
    $providerKey.select2({
        ajax: {
            url: '/api/permission-management/permissions/search-resource-provider-keys',
            delay: 250,
            dataType: "json",
            data: function (params) {
                var query = {};
                query["resourceName"] = $('#ResourceName').val();
                query["serviceName"] = $('input[name="AddModel.ProviderName"]:checked').val();
                query["page"] = params.page || 1;
                query["filter"] = params.term;
                return query;
            },
            processResults: function (data) {
                var keyValues = [];
                data.keys.forEach(function (item, index) {
                    keyValues.push({
                        id: item["providerKey"],
                        text: item["providerDisplayName"],
                        displayName: item["providerDisplayName"]
                    })
                });
                return {
                    results: keyValues,
                    pagination: {
                        more: keyValues.length == 10
                    }
                };
            }
        },
        width: '100%',
        dropdownParent: $('#addResourcePermissionManagementModal'),
        language: abp.localization.currentCulture.cultureName
    });

    $('input[name="AddModel.ProviderName"]').change(function () {
        $providerKey.val(null).trigger('change');
    });

    $providerKey.change(function () {
        if ($providerKey.val()) {
            $permissionManagementForm.valid();
        }
        var providerKey = $providerKey.val();
        if (!providerKey) {
            $items.prop('checked', false);
            $all.prop("checked", false);
            return;
        }

        abp.ui.setBusy('#permissionList');
        var resourceName = $("#ResourceName").val();
        var resourceKey = $("#ResourceKey").val();
        var providerName = $('input[name="AddModel.ProviderName"]:checked').val();
        volo.abp.permissionManagement.permissions.getResourceByProvider(resourceName, resourceKey, providerName, providerKey).then(function (result) {
            abp.ui.clearBusy();
            var grantedPermissionNames = result.permissions.filter(function (p) {
                return p.providers.indexOf(providerName) >= 0 && p.isGranted === true;
            }).map(function (p) {
                return p.name;
            });
            $items.each(function () {
                var $checkbox = $(this);
                if (grantedPermissionNames.indexOf($checkbox.val()) >= 0) {
                    $checkbox.prop('checked', true);
                } else {
                    $checkbox.prop('checked', false);
                }
            });
            $all.prop("checked", $items.length === $items.filter(":checked").length);
        });
    });

    $permissionManagementForm.submit(function () {
        $(this).valid();
    });

})(jQuery);
