(function ($) {

    var l = abp.localization.getResource('AbpIdentity');

    var _identityUserAppService = volo.abp.identity.identityUser;
    var _editModal = new abp.ModalManager(abp.appPath + 'Identity/Users/EditModal');
    var _createModal = new abp.ModalManager(abp.appPath + 'Identity/Users/CreateModal');
    var _permissionsModal = new abp.ModalManager(abp.appPath + 'AbpPermissionManagement/PermissionManagementModal');

    $(function () {

        var _$wrapper = $('#IdentityUsersWrapper');
        var _$table = _$wrapper.find('table');
        var _dataTable = _$table.DataTable(abp.libs.datatables.normalizeConfiguration({
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(_identityUserAppService.getList),
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
                                            providerName: 'User',
                                            providerKey: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: function () {
                                        return true; //TODO: Check permission
                                    },
                                    confirmMessage: function (data) { return l('UserDeletionConfirmationMessage', data.record.userName); },
                                    action: function (data) {
                                        _identityUserAppService
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
                    data: "userName"
                },
                {
                    data: "email"
                },
                {
                    data: "phoneNumber"
                }
            ]
        }));

        _createModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _$wrapper.find('button[name=CreateUser]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });
    });

})(jQuery);
