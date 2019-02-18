var abp = abp || {};
(function ($) {
    abp.modals = abp.modals || {};

    abp.modals.PermissionManagement = function () {
        function checkParents($tab, $checkBox) {
            var parentName = $checkBox.closest('.form-check').attr('data-parent-name');
            if (!parentName) {
                return;
            }

            $tab.find('.form-check')
                .filter('[data-permission-name="' + parentName + '"]')
                .find('input[type="checkbox"]')
                .each(function() {
                    var $parent = $(this);
                    $parent.prop('checked', true);
                    checkParents($tab, $parent);
                });
        }

        function uncheckChildren($tab, $checkBox) {
            var permissionName = $checkBox.closest('.form-check').attr('data-permission-name');
            if (!permissionName) {
                return;
            }

            $tab.find('.form-check')
                .filter('[data-parent-name="' + permissionName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    var $child = $(this);
                    $child.prop('checked', false);
                    uncheckChildren($tab, $child);
                });
        }

        this.initDom = function($el) {
            $el.find('.tab-pane').each(function () {
                var $tab = $(this);
                $tab.find('input[type="checkbox"]').each(function () {
                    var $checkBox = $(this);
                    $checkBox.change(function () {
                        if ($checkBox.is(':checked')) {
                            checkParents($tab, $checkBox);
                        } else {
                            uncheckChildren($tab, $checkBox);
                        }
                    });
                });
                $('input:radio[name="ShowOnlyProviderPermissonsToggle"]').change(
                    function () {
                        if ($(this).val() == '1') {
                            $('.HideOnProviderToggle').slideUp();
                            $('.hideTabOnToggle').slideUp();
                        }
                        else {
                            $('.HideOnProviderToggle').slideDown();
                            $('.hideTabOnToggle').slideDown();
                        }
                    });
            });
        };
    };
})(jQuery);