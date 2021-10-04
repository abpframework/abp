var abp = abp || {};
(function ($) {
    abp.modals = abp.modals || {};

    abp.modals.FeatureManagement = function () {
        function checkParents($tab, $element, className) {
            var parentName = $element
                .closest(className)
                .attr('data-parent-name');

            if (!parentName) {
                return;
            }

            $tab.find('.custom-checkbox')
                .filter('[data-feature-name="' + parentName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    var $parent = $(this);
                    $parent.prop('checked', true);
                    checkParents($tab, $parent, className);
                });
        }

        function uncheckChildren($tab, $checkBox) {
            var featureName = $checkBox
                .closest('.custom-checkbox')
                .attr('data-feature-name');
            if (!featureName) {
                return;
            }

            $tab.find('.custom-checkbox')
                .filter('[data-parent-name="' + featureName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    var $child = $(this);
                    $child.prop('checked', false);
                    uncheckChildren($tab, $child);
                });
        }

        this.initDom = function ($el) {
            $el.find('.tab-pane').each(function () {
                var $tab = $(this);
                $tab.find('input[type="checkbox"]')
                    .each(function () {
                        var $checkBox = $(this);
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
                        var $element = $(this);
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
