var abp = abp || {};
(function ($) {
    abp.modals = abp.modals || {};

    let l = abp.localization.getResource("AbpFeatureManagement");
    abp.modals.FeatureManagement = function () {
        abp.ResourceLoader.loadScript('/client-proxies/featureManagement-proxy.js');
        $('#ResetToDefaults').click(function (e) {
            abp.message.confirm(l('AreYouSureToResetToDefault'))
                .then(function (confirmed) {
                    if (confirmed) {
                        let providerName = $('#ProviderName').val();
                        let prodiverKey = $('#ProviderKey').val();
                        volo.abp.featureManagement.features.delete(providerName, prodiverKey).then(function () {
                            abp.notify.success(l('ResetedToDefault'));
                        });
                        setTimeout(function () {
                            $('#featureManagmentModal').modal('hide');
                        }, 500);
                    }
                });
        });

        function checkParents($tab, $element, className) {
            let parentName = $element
                .closest(className)
                .attr('data-parent-name');

            if (!parentName) {
                return;
            }

            $tab.find('.custom-checkbox')
                .filter('[data-feature-name="' + parentName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    let $parent = $(this);
                    $parent.prop('checked', true);
                    checkParents($tab, $parent, className);
                });
        }

        function uncheckChildren($tab, $checkBox) {
            let featureName = $checkBox
                .closest('.custom-checkbox')
                .attr('data-feature-name');
            if (!featureName) {
                return;
            }

            $tab.find('.custom-checkbox')
                .filter('[data-parent-name="' + featureName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    let $child = $(this);
                    $child.prop('checked', false);
                    uncheckChildren($tab, $child);
                });
        }

        this.initDom = function ($el) {
            $el.find('.tab-pane').each(function () {
                let $tab = $(this);
                $tab.find('input[type="checkbox"]')
                    .each(function () {
                        let $checkBox = $(this);
                        $checkBox.change(function () {
                            if ($checkBox.is(':checked')) {
                                checkParents($tab, $checkBox, '.custom-checkbox')
                            } else {
                                uncheckChildren($tab, $checkBox);
                            }
                        });
                    });

                $tab.find('.form-control')
                    .each(function () {
                        let $element = $(this);
                        $element.change(function () {
                            checkParents($tab, $element, '.form-group')
                        });
                    });
            });

            $(function () {
                $('.custom-scroll-content').mCustomScrollbar({
                    theme: 'minimal-dark',
                });
                $('.custom-scroll-container > .col-4').mCustomScrollbar({
                    theme: 'minimal-dark',
                });
            });
        };
    };
})(jQuery);
