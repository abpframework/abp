var abp = abp || {};
(function ($) {
    abp.modals = abp.modals || {};

    let l = abp.localization.getResource("AbpFeatureManagement");

    // The toolbars, menus and bundles are rendered on the server, so a full page
    // load is the only way to reflect the new features on the current page. An empty
    // provider key means the features of the current host or tenant were changed.
    function reloadPageIfCurrentFeaturesChanged(providerKey) {
        if (!providerKey) {
            window.location.reload();
        }
    }

    abp.modals.FeatureManagement = function () {

        abp.ResourceLoader.loadScript('/client-proxies/featureManagement-proxy.js');
        $('#ResetToDefaults').click(function (e) {
            abp.message.confirm(l('AreYouSureToResetToDefault'))
                .then(function (confirmed) {
                    if (confirmed) {
                        let providerName = $('#ProviderName').val();
                        let prodiverKey = $('#ProviderKey').val();
                        volo.abp.featureManagement.features.delete(providerName, prodiverKey).then(function () {
                            $("#FeatureManagementForm").get(0).reset();
                            abp.notify.success(l('SavedSuccessfully'));
                            $('#featureManagmentModal').modal('hide');
                            reloadPageIfCurrentFeaturesChanged(prodiverKey);
                        });
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
            let initialValues = $el.serialize();

            $el.on('abp-ajax-success', function () {
                if ($el.serialize() !== initialValues) {
                    reloadPageIfCurrentFeaturesChanged($el.find('#ProviderKey').val());
                }
            });

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
        };
    };
})(jQuery);
