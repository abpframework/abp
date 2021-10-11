
$(function () {
    var l = abp.localization.getResource("CmsKit");

    var createModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Blogs/CreateModal", modalClass: 'createBlog' });
    var updateModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Blogs/UpdateModal", modalClass: 'updateBlog' });
    var featuresModal = new abp.ModalManager(abp.appPath + "CmsKit/Blogs/FeaturesModal");

    var blogsService = volo.cmsKit.admin.blogs.blogAdmin;

    var dataTable = $("#BlogsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollCollapse: true,
        scrollX: true,
        ordering: true,
        order: [[1, "desc"]],
        ajax: abp.libs.datatables.createAjax(blogsService.getList),
        columnDefs: [
            {
                title: l("Details"),
                targets: 0,
                rowAction: {
                    items: [
                        {
                            text: l('Features'),
                            visible: abp.auth.isGranted('CmsKit.Blogs.Features'),
                            action: function (data) {
                                featuresModal.open({ blogId: data.record.id });
                            }
                        },
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.Blogs.Update'),
                            action: function (data) {
                                updateModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.Blogs.Delete'),
                            confirmMessage: function (data) {
                                return l("BlogDeletionConfirmationMessage", data.record.name)
                            },
                            action: function (data) {
                                blogsService
                                    .delete(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reload();
                                    });
                            }
                        }
                    ]
                }
            },
            {
                title: l("Name"),
                orderable: true,
                data: "name"
            },
            {
                title: l("Slug"),
                orderable: true,
                data: "slug"
            }
        ]
    }));

    $('#AbpContentToolbar button[name=CreateBlog]').on('click', function (e) {
        e.preventDefault();
        createModal.open();
    });

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    updateModal.onResult(function () {
        dataTable.ajax.reload();
    });
});