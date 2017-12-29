(function () {

    var l = abp.localization.getResource('AbpIdentity');
    var _identityRoleAppService = volo.abp.identity.identityRole;

    var _editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Identity/Roles/EditModal'
    });

    var _createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Identity/Roles/CreateModal'
    });

    var app = new Vue({
        el: '#IdentityRolesWrapper',
        methods: {
            openCreateModal: function () {
                _createModal.open();
            }
        }
    });

    $(function () {

        var _$wrapper = $('#IdentityRolesWrapper');
        var _$table = _$wrapper.find('table');

        var _dataTable = _$table.DataTable({
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(_identityRoleAppService.getList),
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        text: '<i class="fa fa-cog"></i> ' + l('Actions') + ' <span class="caret"></span>',
                        items:
                        [
                            {
                                text: l('Edit'),
                                visible: function () {
                                    return true;
                                },
                                action: function (data) {
                                    _editModal.open({
                                        id: data.record.id
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: function () {
                                    return true;
                                },
                                action: function (data) {
                                    if (confirm(l('UserDeletionConfirmationMessage', data.record.name))) {
                                        _identityRoleAppService
                                            .delete(data.record.id)
                                            .then(function () {
                                                _dataTable.ajax.reload();
                                            });
                                    }
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "name"
                }
            ]
        });

        _createModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reload();
        });
    });

})();
