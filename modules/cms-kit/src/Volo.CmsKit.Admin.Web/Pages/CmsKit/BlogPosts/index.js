
$(function () {
    var l = abp.localization.getResource("CmsKit");

    var blogsService = volo.cmsKit.admin.blogs.blogPostAdmin;

    var $blogPostWrapper = $('#CmsKitBlogPostsWrapper');

    var dataTable = $("#BlogPostsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
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
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.Blogs.Update'),
                            action: function (data) {
                                location.href = "BlogPosts/Update/" + data.record.id
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.Blogs.Delete'),
                            confirmMessage: function (data) {
                                return l("BlogPostDeletionConfirmationMessage", data.record.title)
                            },
                            action: function (data) {
                                blogsService
                                    .delete(data.record.id)
                                    .then(function () {
                                        abp.notify.info(l("SuccessfullyDeleted"));
                                        dataTable.ajax.reload();
                                    });
                            }
                        }
                    ]
                }
            },
            {
                title: l("Title"),
                orderable: true,
                data: "title"
            },
            {
                title: l("Slug"),
                orderable: true,
                data: "slug"
            }
        ]
    }));

    $('#AbpContentToolbar button[name=CreateBlogPost]').on('click', function (e) {
        e.preventDefault();
        window.location.href = "BlogPosts/Create"
    });
});