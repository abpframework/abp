var _menuItem = {};
$(function () {
    var l = abp.localization.getResource("CmsKit");

    var menuService = volo.cmsKit.admin.menus.menuItemAdmin;

    var createModal = new abp.ModalManager({ viewUrl: abp.appPath + 'CmsKit/Menus/MenuItems/CreateModal', modalClass: 'createMenuItem' });
    var updateModal = new abp.ModalManager({ viewUrl: abp.appPath + 'CmsKit/Menus/MenuItems/UpdateModal', modalClass: 'updateMenuItem'});

    var menuTree = {

        $tree: $('#MenuItemsEditTree'),


        $emptyInfo: $('#MenuItemsTreeEmptyInfo'),

        show: function () {
            menuTree.$emptyInfo.hide();
            menuTree.$tree.show();
        },

        hide: function () {
            menuTree.$emptyInfo.show();
            menuTree.$tree.hide();
        },

        itemCount: 0,

        setItemCount: function (itemCount) {
            menuTree.itemCount = itemCount;
            if (itemCount) {
                menuTree.show();
            } else {
                menuTree.hide();
            }
        },

        refreshItemCount: function () {
            menuTree.setItemCount(menuTree.$tree.jstree('get_json').length);
        },

        selectedMenuItem: {
            id: null,
            displayName: null,
            code: null,

            set: function (menuItemOnTree) {
                if (!menuItemOnTree) {
                    menuTree.selectedMenuItem.id = null;
                    menuTree.selectedMenuItem.displayName = null;
                    menuTree.selectedMenuItem.IsActive = true;
                    menuTree.selectedMenuItem.Url = null;
                    menuTree.selectedMenuItem.Icon = null;
                    menuTree.selectedMenuItem.Order = 1;
                    menuTree.selectedMenuItem.Target = null;
                    menuTree.selectedMenuItem.ElementId = null;
                    menuTree.selectedMenuItem.CssClass = null;
                    menuTree.selectedMenuItem.PageId = null;
                } else {
                    menuTree.selectedMenuItem.id = menuItemOnTree.id;
                    menuTree.selectedMenuItem.displayName = menuItemOnTree.original.displayName;
                    menuTree.selectedMenuItem.IsActive = menuItemOnTree.original.IsActive;
                    menuTree.selectedMenuItem.Url = menuItemOnTree.original.Url;
                    menuTree.selectedMenuItem.Icon = menuItemOnTree.original.Icon;
                    menuTree.selectedMenuItem.Order = menuItemOnTree.original.Order;
                    menuTree.selectedMenuItem.Target = menuItemOnTree.original.Target;
                    menuTree.selectedMenuItem.ElementId = menuItemOnTree.original.ElementId;
                    menuTree.selectedMenuItem.CssClass = menuItemOnTree.original.CssClass;
                    menuTree.selectedMenuItem.PageId = menuItemOnTree.original.PageId;
                }

                _menuItem.checkbox.setSelectedItem({
                    id: menuTree.selectedMenuItem.id,
                    displayName: menuTree.selectedMenuItem.displayName,
                    isActive: menuTree.selectedMenuItem.IsActive,
                    url: menuTree.selectedMenuItem.Url,
                    icon: menuTree.selectedMenuItem.Icon,
                    order: menuTree.selectedMenuItem.Order,
                    target: menuTree.selectedMenuItem.Target,
                    elementId: menuTree.selectedMenuItem.ElementId,
                    cssClass: menuTree.selectedMenuItem.CssClass,
                    pageId: menuTree.selectedMenuItem.PageId
                });
            }
        },

        contextMenu: function (node) {
            var items = {
                edit: {
                    label: l('Edit'),
                    icon: 'fa fa-pencil',
                    _disabled: !abp.auth.isGranted('CmsKit.Menus.Update'),
                    action: function (data) {
                        var instance = $.jstree.reference(data.reference);

                        updateModal.open({
                            id: node.id
                        })
                    }
                },

                addSubMenuItem: {
                    label: l('AddSubMenuItem'),
                    icon: 'fa fa-plus',
                    _disabled: !abp.auth.isGranted('CmsKit.Menus.Update'),
                    action: function () {
                        menuTree.addItem(node.id);
                    }
                },

                'delete': {
                    label: l('Delete'),
                    icon: 'fa fa-remove',
                    _disabled: !abp.auth.isGranted('CmsKit.Menus.Update'),
                    action: function (data) {
                        var instance = $.jstree.reference(data.reference);

                        abp.message.confirm(
                            l('MenuItemDeletionConfirmationMessage', node.original.displayName),
                            l('AreYouSure'),
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    menuService
                                        .delete(node.id)
                                        .done(function () {
                                            instance.delete_node(node);
                                            menuTree.refreshItemCount();
                                        })
                                        .fail(function (err) {
                                            setTimeout(function () {
                                                abp.message.error(err.message);
                                            }, 500);
                                        });
                                }
                            }
                        )
                    }
                }
            }
            return items;
        },

        addItem: function (parentId) {
            var instance = $.jstree.reference(menuTree.$tree);

            createModal.open({
                parentId: parentId,
            }, function (newMenuItem) {
                instance.create_node(
                    parentId ? instance.get_node(parentId) : '#',
                    {
                        id: newMenuItem.id,
                        parent: newMenuItem.parentId ? newMenuItem.parentId : '#',
                        isActive: newMenuItem.isActive,
                        displayName: $.fn.dataTable.render.text().display(newMenuItem.displayName),
                        text: menuTree.generateTextOnTree(newMenuItem),
                        url: newMenuItem.url,
                        icon: newMenuItem.icon,
                        order: newMenuItem.order,
                        target: newMenuItem.target,
                        elementId: newMenuItem.elementId,
                        cssClass: newMenuItem.cssClass,
                        pageId: newMenuItem.pageId,
                        state: {
                            opened: true
                        }
                    });

                menuTree.refreshItemCount();
            });
        },

        generateTextOnTree: function (item) {
            var itemClass = ' ou-text-no-members';
            return '<span title="' + item.displayName + '" class="ou-text text-dark' + itemClass + '" data-menuitem-id="' + item.id + '">' +
                $.fn.dataTable.render.text().display(item.displayName) +
                ' <i class="fa fa-caret-down text-muted"></i></span > ';
        },

        getTreeDataFromServer: function (callback) {
            menuService.getList().done(function (result) {
                var treeData = _.map(result.items, function (item) {
                    return {
                        id: item.id,
                        parent: item.parentId ? item.parentId : '#',
                        isActive: item.isActive,
                        displayName: $.fn.dataTable.render.text().display(item.displayName),
                        url: item.url,
                        icon: item.icon,
                        order: item.order,
                        target: item.target,
                        elementId: item.elementId,
                        cssClass: item.cssClass,
                        pageId: item.pageId,
                        text: menuTree.generateTextOnTree(item),
                        state: {
                            opened: true
                        }
                    };
                });

                callback(treeData);
            });
        },

        init: function () {
            menuTree.getTreeDataFromServer(function (treeData) {

                menuTree.setItemCount(treeData.length);

                menuTree.$tree
                    .on('changed.jstree', function (e, data) {
                        if (data.selected.length != 1) {
                            menuTree.selectedMenuItem.set(null);
                        } else {
                            var selectedNode = data.instance.get_node(data.selected[0]);
                            menuTree.selectedMenuItem.set(selectedNode);
                        }
                    })
                    .on('move_node.jstree', function (e, data) {

                        var parentNodeName = (!data.parent || data.parent == '#')
                            ? l('Root')
                            : menuTree.$tree.jstree('get_node', data.parent).original.displayName;

                        abp.message.confirm(
                            l('MenuItemMoveConfirmMessage', data.node.original.displayName, parentNodeName),
                            l('AreYouSure'),
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    menuService
                                        .moveMenuItem(data.node.id, {
                                            newParentId: data.parent === '#' ? null : data.parent,
                                            position: data.position
                                        })
                                        .done(function () {
                                            menuTree.reload();
                                        })
                                        .fail(function (err) {
                                            menuTree.$tree.jstree('refresh'); //rollback
                                            setTimeout(function () {
                                                abp.message.error(err.message);
                                            }, 500);
                                        });
                                } else {
                                    menuTree.$tree.jstree('refresh'); //rollback
                                }
                            }
                        );
                    })
                    .jstree({
                        'core': {
                            data: treeData,
                            multiple: false,
                            check_callback: function (operation, node, node_parent, node_position, more) {
                                return true;
                            }
                        },

                        contextmenu: {
                            items: menuTree.contextMenu,
                            'select_node': false
                        },

                        sort: function (node1, node2) {
                           if (this.get_node(node2).original.order < this.get_node(node1).original.order) {
                               return 1;
                           }

                           return -1;
                        },

                        plugins: [
                            'types',
                            'contextmenu',
                            'wholerow',
                            'sort',
                            'dnd'
                        ]
                    });

                $('button[name=CreateMenuItem]').click(function (e) {
                    e.preventDefault();
                    createModal.open();
                });

                createModal.onResult(function () {
                    menuTree.reload();
                });

                updateModal.onResult(function (item) {
                    menuTree.reload();
                });

                menuTree.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                   e.preventDefault();
                   var id = $(this).closest('.ou-text').attr('data-menuitem-id');
                   setTimeout(function () {
                       menuTree.$tree.jstree('show_contextmenu', id);
                   }, 100);
                });
            });
        },

        reload: function () {
            menuTree.getTreeDataFromServer(function (treeData) {
                menuTree.setItemCount(treeData.length);
                menuTree.$tree.jstree(true).settings.core.data = treeData;
                menuTree.$tree.jstree('refresh');
            });
        }
    };

    menuTree.init();

    let selectedIds = [];
    let selectedItem = {};
    let selectedCheckboxNames;
    let selectAllHeaderTitle = '<div class="custom-checkbox custom-control no-height">' +
        '<input type="checkbox" id="select_all" name="select-all" class="custom-control-input custom-selectbox" value="false" data-val="false">' +
        '<label class="custom-control-label" for="select_all"></label></div>';

    _menuItem.checkbox = {
        initialize: function (selectedCbNames) {
            selectedIds = [];
            selectedCheckboxNames = selectedCbNames;
        },
        getSelectedIds: function () {
            return selectedIds;
        },
        setSelectedItem: function (item) {
            selectedItem = item;
        },
        getSelectedItem: function () {
            return selectedItem;
        },
        getSelectAllHeaderTitle: function () {
            return selectAllHeaderTitle;
        }
    };

    $(document).on('click', '.custom-selectbox', function (el) {
        el.stopPropagation();
        if (!$(el.target).is(':checkbox')) {
            return;
        }

        if ($(el.target).prop("checked") == true) {
            checkAndAddAll();
        } else if ($(el.target).prop("checked") == false) {
            uncheckAndRemoveAll();
        }
    });

    $(document).on('click', '.selectable', function (el) {
        el.stopPropagation();
        selectDeselect(el);
    });

    const checkAndAddAll = function () {
        $('table>tbody>').find('input[type="checkbox"]').prop("checked", true);
        let selectedItems = $(`input[name="${selectedCheckboxNames}"]:checked`);
        selectedIds = [];
        for (var i = 0; typeof (selectedItems[i]) != 'undefined'; selectedIds.push(selectedItems[i++].getAttribute('id'))) ;
    };

    const uncheckAndRemoveAll = function () {
        $('table>tbody>').find('input[type="checkbox"]').prop("checked", false);
        selectedIds = [];
    };

    const addId = function (id) {
        selectedIds.push(id);
    };

    const removeId = function (id) {
        selectedIds = selectedIds.filter(function (e) {
            return e !== id;
        });
    };

    const selectDeselect = function (el) {
        $el = $(el);
        if ($(el.target).is(':checkbox')) {
            el.preventDefault();
            return;
        }

        if (!$(el.target).is(':checkbox')) {
            $el = $(el.target).parent().closest('tr').find('[type=checkbox]');

            if ($el.prop("checked") == true) {
                $el.prop("checked", false);
            } else if ($el.prop("checked") == false) {
                $el.prop("checked", true);
            }
        }

        if ($el.prop("checked") == true) {
            addId($el.attr('id'));
        } else if ($el.prop("checked") == false) {
            removeId($el.attr('id'));
        }
    }
});
