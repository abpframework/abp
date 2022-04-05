(function ($) {
    var l = abp.localization.getResource('AbpIdentity');

    var _identityUserAppService = volo.abp.identity.identityUser;

    var togglePasswordVisibility = function () {
        $("#PasswordVisibilityButton").click(function (e) {
            var button = $(this);
            var passwordInput = button.parent().find("input");
            if(!passwordInput) {
                return;
            }

            if(passwordInput.attr("type") === "password") {
                passwordInput.attr("type", "text");
            }
            else {
                passwordInput.attr("type", "password");
            }

            var icon = button.find("i");
            if(icon) {
                icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
            }
        });
    }
    
    abp.modals.createUser = function () {
        var initModal = function (publicApi, args) {
            togglePasswordVisibility();
        };
        return { initModal: initModal };
    }
    
    abp.modals.editUser = function () {
        var initModal = function (publicApi, args) {
            togglePasswordVisibility();
        };
        return { initModal: initModal };
    }
    
    var _editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Identity/Users/EditModal',
        modalClass: "editUser"
    });
    var _createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Identity/Users/CreateModal',
        modalClass: "createUser"
    });
    var _permissionsModal = new abp.ModalManager(
        abp.appPath + 'AbpPermissionManagement/PermissionManagementModal'
    );

    var _dataTable = null;

    abp.ui.extensions.entityActions.get('identity.user').addContributor(
        function(actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted(
                            'AbpIdentity.Users.Update'
                        ),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Permissions'),
                        visible: abp.auth.isGranted(
                            'AbpIdentity.Users.ManagePermissions'
                        ),
                        action: function (data) {
                            _permissionsModal.open({
                                providerName: 'U',
                                providerKey: data.record.id,
                                providerKeyDisplayName: data.record.userName
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        visible: function(data) {
                            return abp.auth.isGranted('AbpIdentity.Users.Delete') && abp.currentUser.id !== data.id;
                        },
                        confirmMessage: function (data) {
                            return l(
                                'UserDeletionConfirmationMessage',
                                data.record.userName
                            );
                        },
                        action: function (data) {
                            _identityUserAppService
                                .delete(data.record.id)
                                .then(function () {
                                    _dataTable.ajax.reload();
                                    abp.notify.success(l('SuccessfullyDeleted'));
                                });
                        },
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get('identity.user').addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get('identity.user').actions.toArray()
                        }
                    },
                    {
                        title: l('UserName'),
                        data: 'userName',
                        render: function (data, type, row) {
                            row.userName = $.fn.dataTable.render.text().display(row.userName);
                            if (!row.isActive) {
                                return  '<i data-toggle="tooltip" data-placement="top" title="' +
                                    l('ThisUserIsNotActiveMessage') +
                                    '" class="fa fa-ban text-danger"></i> ' +
                                    '<span class="opc-65">' + row.userName + '</span>';
                            }

                            return row.userName;
                        }
                    },
                    {
                        title: l('EmailAddress'),
                        data: 'email',
                    },
                    {
                        title: l('PhoneNumber'),
                        data: 'phoneNumber',
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    $(function () {
        var _$wrapper = $('#IdentityUsersWrapper');
        var _$table = _$wrapper.find('table');
        _dataTable = _$table.DataTable(
            abp.libs.datatables.normalizeConfiguration({
                order: [[1, 'asc']],
                processing: true,
                serverSide: true,
                scrollX: true,
                paging: true,
                ajax: abp.libs.datatables.createAjax(
                    _identityUserAppService.getList
                ),
                columnDefs: abp.ui.extensions.tableColumns.get('identity.user').columns.toArray()
            })
        );

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
