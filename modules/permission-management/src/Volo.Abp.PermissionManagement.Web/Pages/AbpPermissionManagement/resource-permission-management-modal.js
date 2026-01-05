var abp = abp || {};
(function ($) {
    var l = abp.localization.getResource('AbpPermissionManagement');
    var _dataTable = null;
    abp.ui.extensions.entityActions.get('permissionManagement.resource').addContributor(
        function (actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        action: function (data) {
                            var _updateResourcePermissionsModal = new abp.ModalManager(abp.appPath + "AbpPermissionManagement/UpdateResourcePermissionManagementModal");
                            _updateResourcePermissionsModal.open({
                                resourceName: $("#ResourceName").val(),
                                resourceKey: $("#ResourceKey").val(),
                                resourceDisplayName: $("#ResourceDisplayName").val(),
                                providerName: data.record.providerName,
                                providerKey: data.record.providerKey
                            });
                            _updateResourcePermissionsModal.onResult(function () {
                                _dataTable.ajax.reloadEx(function (json) {
                                    _dataTable.columns.adjust();
                                });
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        confirmMessage: function (data) {
                            return l(
                                'ResourcePermissionDeletionConfirmationMessage',
                                data.record.name
                            );
                        },
                        action: function (data) {
                            volo.abp.permissionManagement.permissions.deleteResource($("#ResourceName").val(), $("#ResourceKey").val(), data.record.providerName, data.record.providerKey).then(function () {
                                abp.notify.info(l('DeletedSuccessfully'));
                                _dataTable.ajax.reloadEx(function (json) {
                                    _dataTable.columns.adjust();
                                });
                            });
                        },
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get('permissionManagement.resource').addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get('permissionManagement.resource').actions.toArray()
                        }
                    },
                    {
                        title: l("ResourcePermissionTarget"),
                        data: 'providerName',
                        render: function (data, type, row) {
                            return '<span class="d-inline-block bg-light rounded-pill px-2 me-1 ms-1 mb-1" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-title="' + row.providerNameDisplayName + '">' + row.providerName + '</span>' + row.providerDisplayName;
                        },
                    },
                    {
                        title: l("ResourcePermissionPermissions"),
                        data: 'permissions',
                        render: function (data, type, row) {
                            var spans = '';
                            for (var i = 0; i < row.permissions.length; i++) {
                                spans += '<span class="d-inline-block bg-light rounded-pill px-2 me-1 mb-1">' + row.permissions[i].displayName + '</span>';
                            }
                            return spans;
                        },
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    abp.modals = abp.modals || {};

    abp.modals.ResourcePermissionManagement = function () {
        var initModal = function (publicApi, args) {
            _dataTable = $('#resourcePermissionTable').DataTable(
                abp.libs.datatables.normalizeConfiguration({
                    order: [],
                    searching: false,
                    processing: true,
                    scrollX: false,
                    serverSide: false,
                    paging: true,
                    ajax: function () {
                        return function (requestData, callback, settings) {
                            if (callback) {
                                volo.abp.permissionManagement.permissions.getResource(args.resourceName, args.resourceKey).then(function (result) {
                                    callback({
                                        recordsTotal: result.permissions.length,
                                        recordsFiltered: result.permissions.length,
                                        data: result.permissions
                                    });
                                });
                            }
                        }
                    }(),
                    columnDefs: abp.ui.extensions.tableColumns.get('permissionManagement.resource').columns.toArray(),
                })
            );

            $("#addPermission").click(function () {
                var _addResourcePermissionsModal = new abp.ModalManager(abp.appPath + "AbpPermissionManagement/AddResourcePermissionManagementModal");
                _addResourcePermissionsModal.open({
                    resourceName: $("#ResourceName").val(),
                    resourceKey: $("#ResourceKey").val(),
                    resourceDisplayName: $("#ResourceDisplayName").val()
                });
                _addResourcePermissionsModal.onResult(function () {
                    _dataTable.ajax.reloadEx(function (json) {
                        _dataTable.columns.adjust();
                    });
                });
            });
        };

        return {
            initModal: initModal,
        };
    };
})(jQuery);
