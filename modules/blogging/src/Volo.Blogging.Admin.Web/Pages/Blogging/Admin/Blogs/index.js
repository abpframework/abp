$(function () {
    var l = abp.localization.getResource('Blogging');
    var _createModal = new abp.ModalManager(
        abp.appPath + 'Blogging/Admin/Blogs/Create'
    );
    var _editModal = new abp.ModalManager(
        abp.appPath + 'Blogging/Admin/Blogs/Edit'
    );

    var _dataTable = $('#BlogsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: false,
            info: false,
            scrollX: true,
            searching: false,
            autoWidth: false,
            scrollCollapse: true,
            order: [[3, 'desc']],
            ajax: abp.libs.datatables.createAjax(
                volo.blogging.admin.blogManagement.getList
            ),
            columnDefs: [
                {
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted(
                                    'Blogging.Blog.Update'
                                ),
                                action: function (data) {
                                    _editModal.open({
                                        blogId: data.record.id,
                                    });
                                },
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted(
                                    'Blogging.Blog.Delete'
                                ),
                                confirmMessage: function (data) {
                                    return l('BlogDeletionWarningMessage');
                                },
                                action: function (data) {
                                    volo.blogging.admin.blogManagement
                                        .delete(data.record.id)
                                        .then(function () {
                                            _dataTable.ajax.reload();
                                        });
                                },
                            },
                            {
                                text: l("ClearCache"),
                                visible: abp.auth.isGranted(
                                  'Blogging.Blog.ClearCache'  
                                ),
                                confirmMessage: function (data) {
                                    return l("ClearCacheConfirmationMessage");
                                },
                                action: function (data) {
                                    volo.blogging.admin.blogManagement
                                        .clearCache(data.record.id)
                                        .then(function () {
                                            _dataTable.ajax.reload();
                                        })
                                }
                            }
                        ],
                    },
                },
                {
                    target: 1,
                    data: 'name',
                },
                {
                    target: 2,
                    data: 'shortName',
                },
                {
                    target: 3,
                    data: 'creationTime',
                    dataFormat: "datetime"
                },
                {
                    target: 4,
                    data: 'description',
                },
            ],
        })
    );

    $('#CreateNewBlogButtonId').click(function () {
        _createModal.open();
    });

    _createModal.onClose(function () {
        _dataTable.ajax.reload();
    });

    _editModal.onResult(function () {
        _dataTable.ajax.reload();
    });
});
