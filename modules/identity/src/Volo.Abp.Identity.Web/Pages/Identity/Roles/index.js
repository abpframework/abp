﻿(function ($) {

    var l = abp.localization.getResource('AbpIdentity');

    var _identityRoleAppService = volo.abp.identity.identityRole;
    var _permissionsModal = new abp.ModalManager(abp.appPath + 'AbpPermissionManagement/PermissionManagementModal');
    var _editModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/EditModal');
    var _createModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/CreateModal');

    $(function () {

        var _$wrapper = $('#IdentityRolesWrapper');
        var _$table = _$wrapper.find('table');

        var _dataTable = _$table.DataTable(abp.libs.datatables.normalizeConfiguration({
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(_identityRoleAppService.getList),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: function () {
                                        return true; //TODO: Check permission
                                    },
                                    action: function (data) {
                                        _editModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l('Permissions'),
                                    visible: function () {
                                        return true; //TODO: Check permission
                                    },
                                    action: function (data) {
                                        _permissionsModal.open({
                                            providerName: 'Role',
                                            providerKey: data.record.name
                                        });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: function (data) {
                                        return !data.isStatic; //TODO: Check permission
                                    },
                                    confirmMessage: function (data) { return l('RoleDeletionConfirmationMessage', data.record.name)},
                                    action: function (data) {
                                        _identityRoleAppService
                                            .delete(data.record.id)
                                            .then(function () {
                                                _dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    data: "name"
                }
            ]
        }));

        _createModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _$wrapper.find('button[name=CreateRole]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });
    });

})(jQuery);
