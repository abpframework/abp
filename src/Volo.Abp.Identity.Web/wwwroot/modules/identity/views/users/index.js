$(function () {
    var identityUserAppService = volo.abp.identity.identityUser;

    var dataTable = $('#IdentityUsersTable').DataTable({
        processing: true,
        serverSide: true,
        ajax: function (requestData, callback, settings) {
            var inputFilter = {};

            //set paging filters
            if (settings.oInit.paging) {
                inputFilter = $.extend(inputFilter, {
                    maxResultCount: requestData.length,
                    skipCount: requestData.start
                });
            }

            //set sorting filter
            if (requestData.order && requestData.order.length > 0) {
                var orderingField = requestData.order[0];
                if (requestData.columns[orderingField.column].data) {
                    inputFilter.sorting = requestData.columns[orderingField.column].data + " " + orderingField.dir;
                }
            }

            //set searching filter
            if (requestData.search && requestData.search.value !== "") {
                inputFilter.filter = requestData.search.value;
            }

            if (callback) {
                identityUserAppService.getList(inputFilter).done(function (result) {
                    callback({
                        recordsTotal: result.totalCount,
                        recordsFiltered: result.totalCount,
                        data: result.items
                    });
                });
            }
        },
        responsive: true,
        columnDefs: [
            {
                targets: 0,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                render: function (list, type, record, meta) {
                    return '<div class="dropdown">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
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

    $(document).on('click', '.update-user', function () {
        var id = $(this).data('id');

        $('#createUpdateUserModal').modal('show')
            .find('.modal-content')
            .load(abp.appPath + 'Identity/Users/_Update', { id: id });
    });

    $(document).on('click', '.delete-user', function () {
        var id = $(this).data('id');

        if (confirm('Are you sure you want to delete?')) {
            identityUserAppService.delete(id).done(function () {
                dataTable.ajax.reload();
            });
        }
    });

    $('.create-user').click(function () {
        $('#createUpdateUserModal').modal('show')
            .find('.modal-content')
            .load(abp.appPath + 'Identity/Users/_Create');
    });

    $(document).on('click', '#btnCreateUserSave', function () {
        var $createUserForm = $('#createUserForm');
        var user = $createUserForm.serializeFormToObject();

        identityUserAppService.create(user).done(function () {
            $('#createUpdateUserModal').modal('hide');

            dataTable.ajax.reload();
        });
    });

    $(document).on('click', '#btnUpdateUserSave', function () {
        var $updateUserForm = $('#updateUserForm');
        var user = $updateUserForm.serializeFormToObject();

        identityUserAppService.update(user.Id, user).done(function () {
            $('#createUpdateUserModal').modal('hide');

            dataTable.ajax.reload();
        });
    });
});

$.fn.serializeFormToObject = function () {
    //serialize to array
    var data = $(this).serializeArray();

    //add also disabled items
    $(':disabled[name]', this)
        .each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

    //map to object
    var obj = {};
    data.map(function (x) { obj[x.name] = x.value; });

    return obj;
};
