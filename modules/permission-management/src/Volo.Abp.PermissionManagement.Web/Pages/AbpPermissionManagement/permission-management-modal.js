var abp = abp || {};
(function ($) {
    abp.modals = abp.modals || {};

    abp.modals.PermissionManagement = function () {
        function checkParents($tab, $checkBox) {
            var parentName = $checkBox
                .closest('.custom-checkbox')
                .attr('data-parent-name');
            if (!parentName) {
                return;
            }

            $tab.find('.custom-checkbox')
                .filter('[data-permission-name="' + parentName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    var $parent = $(this);
                    $parent.prop('checked', true);
                    checkParents($tab, $parent);
                });
        }

        function uncheckChildren($tab, $checkBox) {
            var permissionName = $checkBox
                .closest('.custom-checkbox')
                .attr('data-permission-name');
            if (!permissionName) {
                return;
            }

            $tab.find('.custom-checkbox')
                .filter('[data-parent-name="' + permissionName + '"]')
                .find('input[type="checkbox"]')
                .each(function () {
                    var $child = $(this);
                    $child.prop('checked', false);
                    uncheckChildren($tab, $child);
                });
        }

        function handleTabCheckedCheckboxCount($tab) {
            var newCount = 0;
            $tab.find('input[type="checkbox"]')
                .not('[name="SelectAllInThisTab"]')
                .each(function () {
                    if ($(this).is(':checked') === true) {
                        newCount++;
                    }
                });

            var $tabTitle = $('#' + $tab.attr('id') + '-tab');
            var title = $tabTitle
                .html()
                .replace('<b>', '')
                .replace('</b>', '')
                .replace('<small>', '')
                .replace('</small>', '');

            var titleSplitted = title.split(' ');
            if (titleSplitted[titleSplitted.length - 1].startsWith('(')) {
                titleSplitted.pop();
            }
            var titleWithoutCount = titleSplitted.join(' ');
            var newTitle = titleWithoutCount + ' (' + newCount + ')';
            if (newCount > 0) {
                newTitle = '<b>' + newTitle + '</b>';
            } else {
                newTitle = '<small>' + newTitle + '</small>';
            }
            $tabTitle.html(newTitle);
        }

        function handleUncheck($tab) {
            var $checkBox = $tab.find('input[name="SelectAllInThisTab"]');

            if ($checkBox.is(':checked')) {
                if (
                    $tab
                        .find('input[type="checkbox"]')
                        .not('[name="SelectAllInThisTab"]').length > 1
                ) {
                    $($checkBox).prop('indeterminate', true);
                } else {
                    $checkBox.prop('checked', false);
                }
            } else if ($checkBox.is(':indeterminate')) {
                var allUnchecked = true;

                $tab.find('input[type="checkbox"]')
                    .not('[name="SelectAllInThisTab"]')
                    .each(function () {
                        if ($(this).is(':checked') === true) {
                            allUnchecked = false;
                        }
                    });

                if (allUnchecked) {
                    $($checkBox).prop('indeterminate', false);
                    $checkBox.prop('checked', false);
                }
            }
        }

        function handleCheck($tab) {
            var $checkBox = $tab.find('input[name="SelectAllInThisTab"]');

            var allChecked = true;

            $tab.find('input[type="checkbox"]')
                .not('[name="SelectAllInThisTab"]')
                .each(function () {
                    if ($(this).is(':checked') === false) {
                        allChecked = false;
                    }
                });

            if (allChecked) {
                $($checkBox).prop('indeterminate', false);
                $checkBox.prop('checked', true);
            } else {
                $($checkBox).prop('indeterminate', true);
            }
        }

        function initSelectAllInThisTab() {
            var tabs = $('.tab-pane');
            for (var i = 0; i < tabs.length; i++) {
                var $tab = $(tabs[i]);
                var $checkBox = $tab.find('input[name="SelectAllInThisTab"]');

                var allChecked = true;
                var allUnChecked = true;

                $tab.find('input[type="checkbox"]')
                    .not('[name="SelectAllInThisTab"]')
                    .each(function () {
                        if ($(this).is(':checked') === true) {
                            allUnChecked = false;
                        } else {
                            allChecked = false;
                        }
                    });

                if (allChecked) {
                    $($checkBox).prop('checked', true);
                } else if (allUnChecked) {
                    $($checkBox).prop('checked', false);
                } else {
                    $($checkBox).prop('indeterminate', true);
                }
            }
        }

        function setSelectAllInAllTabs() {
            var $checkBox = $('#SelectAllInAllTabs');

            var anyIndeterminate = false;
            var allChecked = true;
            var allUnChecked = true;

            $('input[name="SelectAllInThisTab"]').each(function () {
                if ($(this).is(':checked') === true) {
                    allUnChecked = false;
                } else {
                    allChecked = false;
                }

                if ($(this).is(':indeterminate') === true) {
                    anyIndeterminate = true;
                }
            });

            if (anyIndeterminate) {
                $($checkBox).prop('indeterminate', true);
                return;
            } else {
                $($checkBox).prop('indeterminate', false);
            }

            if (allChecked) {
                $($checkBox).prop('checked', true);
            } else if (allUnChecked) {
                $($checkBox).prop('checked', false);
            } else {
                $($checkBox).prop('indeterminate', true);
            }
        }

        this.initDom = function ($el) {
            $el.find('.tab-pane').each(function () {
                var $tab = $(this);
                handleTabCheckedCheckboxCount($tab);
                $tab.find('input[type="checkbox"]')
                    .not('[name="SelectAllInThisTab"]')
                    .each(function () {
                        var $checkBox = $(this);
                        $checkBox.change(function () {
                            if ($checkBox.is(':checked')) {
                                checkParents($tab, $checkBox);
                                handleCheck($tab);
                            } else {
                                uncheckChildren($tab, $checkBox);
                                handleUncheck($tab);
                            }
                            setSelectAllInAllTabs();
                            handleTabCheckedCheckboxCount($tab);
                        });
                    });
            });

            $('input[name="SelectAllInThisTab"]').change(function () {
                var $checkBox = $(this);
                var $tab = $('#' + $checkBox.attr('data-tab-id'));
                if ($checkBox.is(':checked')) {
                    $tab.find('input[type="checkbox"]')
                        .not(':disabled')
                        .prop('checked', true);
                } else {
                    $tab.find('input[type="checkbox"]')
                        .not(':disabled')
                        .prop('checked', false);
                }
                $($checkBox).prop('indeterminate', false);
                setSelectAllInAllTabs();
                handleTabCheckedCheckboxCount($tab);
            });

            $('input[name="SelectAllInAllTabs"]').change(function () {
                var $checkBox = $(this);
                if ($checkBox.is(':checked')) {
                    $('.tab-pane input[type="checkbox"]')
                        .not(':disabled')
                        .prop('checked', true);
                } else {
                    $('.tab-pane input[type="checkbox"]')
                        .not(':disabled')
                        .prop('checked', false);
                }
                $($checkBox).prop('indeterminate', false);

                $el.find('.tab-pane').each(function () {
                    handleTabCheckedCheckboxCount($(this));
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

            initSelectAllInThisTab();
            setSelectAllInAllTabs();
        };
    };
})(jQuery);
