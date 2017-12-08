$(function () {
    var _l = abp.localization.getResource('AbpIdentityWeb'); //TODO: AbpIdentityWeb to const

    var _identityUserAppService = volo.abp.identity.identityUser;

    var _editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Identity/Users/EditModal'
    });

    var _$wrapper = $('#IdentityUsersWrapper');
    var _$table = _$wrapper.find('table');

    var _dataTable = _$table.DataTable({
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(_identityUserAppService.getList),
        columnDefs: [
            {
                targets: 0,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                render: function (list, type, record, meta) {
                    return '<div class="dropdown">' +
                        '<button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        'Actions' +
                        '</button>' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<a class="dropdown-item update-user" href="#" data-id="' + record.id + '">Edit</a>' +
                        '<a class="dropdown-item delete-user" href="#" data-id="' + record.id + '">Delete</a>' +
                        '</div>' +
                        '</div>';
                }
            },
            {
                targets: 1,
                data: "userName"
            },
            {
                targets: 2,
                data: "email"
            },
            {
                targets: 3,
                data: "phoneNumber"
            }
        ]
    });

    //Update user command
    _$table.on('click', '.update-user', function () { //TODO: To action list!
        _editModal.open({
            id: $(this).data('id')
        });
    });

    //Delete user command
    _$table.on('click', '.delete-user', function () { //TODO: To action list!
        var id = $(this).data('id');
        var userName = $(this).data('userName');

        if (confirm(_l('UserDeletionConfirmationMessage', userName))) {
            _identityUserAppService
                .delete(id)
                .then(function () {
                    _dataTable.ajax.reload();
                });
        }
    });

    //Create user command
    _$wrapper.find('.create-user-button').click(function () {
        $('#createUpdateUserModal').modal('show')
            .find('.modal-content')
            .load(abp.appPath + 'Identity/Users/CreateModal');
    });

    //TODO: btnCreateUserSave and btnUpdateUserSave clicks should be handled in the modals. We may consider to create a model manager as before

    function findAssignedRoleNames() {
        var assignedRoleNames = [];

        $(document).find('.user-role-checkbox-list input[type=checkbox]')
            .each(function () {
                if ($(this).is(':checked')) {
                    assignedRoleNames.push($(this).attr('name'));
                }
            });

        return assignedRoleNames;
    }

    $('#createUpdateUserModal').on('click', '#btnCreateUserSave', function () {
        var $createUserForm = $('#createUserForm');
        var user = $createUserForm.serializeFormToObject();
        user.RoleNames = findAssignedRoleNames();

        _identityUserAppService.create(user).then(function () {
            $('#createUpdateUserModal').modal('hide');
            _dataTable.ajax.reload();
        });
    });

    _editModal.onClose(function() { //TODO: Only refresh if saved!
        _dataTable.ajax.reload();
    });
});