$(function () {

    var l = abp.localization.getResource('Docs');

    var _createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Docs/Admin/Projects/Create',
        modalClass: 'projectCreate'
    });

    var _editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Docs/Admin/Projects/Edit',
        modalClass: 'projectEdit'
    });

    var _pullModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Docs/Admin/Projects/Pull',
        modalClass: 'projectPull'
    });


    var _dataTable = $('#ProjectsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        scrollX: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[2, "desc"]],
        ajax: abp.libs.datatables.createAjax(volo.docs.admin.projectsAdmin.getList),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('Docs.Admin.Projects.Update'),
                                action: function (data) {
                                    _editModal.open({
                                        Id: data.record.id
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('Docs.Admin.Projects.Delete'),
                                confirmMessage: function (data) { return l('ProjectDeletionWarningMessage') },
                                action: function (data) {
                                    volo.docs.admin.projectsAdmin
                                        .delete(data.record.id)
                                        .then(function () {
                                            _dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('Pull'),
                                visible: abp.auth.isGranted('Docs.Admin.Documents'),
                                action: function (data) {
                                    _pullModal.open({
                                        Id: data.record.id
                                    });
                                }
                            }
                        ]
                }
            },
            {
                target: 1,
                data: "name"
            },
            {
                target: 2,
                data: "shortName"
            },
            {
                target: 3,
                data: "documentStoreType"
            },
            {
                target: 4,
                data: "format",
                render: function (data) {
                    if (data === 'md') {
                        return 'markdown';
                    }

                    return data;
                }
            }
        ]
    }));


    $("#CreateNewGithubProjectButtonId").click(function (event) {
        event.preventDefault();
        _createModal.open({source:"GitHub"});
    });

    _createModal.onClose(function () {
        _dataTable.ajax.reload();
    });

    _editModal.onResult(function () {
        _dataTable.ajax.reload();
    });

});