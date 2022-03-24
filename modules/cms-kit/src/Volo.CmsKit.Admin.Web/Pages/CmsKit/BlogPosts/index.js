
$(function () {
    var l = abp.localization.getResource("CmsKit");
    var blogPostStatus = {
        Draft: 0,
        Published: 1
    };
    
    var blogsService = volo.cmsKit.admin.blogs.blogPostAdmin;

    var getFilter = function () {
        return {
            filter: $('#CmsKitBlogPostsWrapper input.page-search-filter-text').val()
        };
    };
    
    var dataTable = $("#BlogPostsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollCollapse: true,
        scrollX: true,
        ordering: true,
        order: [[2, "desc"]],
        ajax: abp.libs.datatables.createAjax(blogsService.getList, getFilter),
        columnDefs: [
            {
                title: l("Details"),
                targets: 0,
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.BlogPosts.Update'),
                            action: function (data) {
                                location.href = "BlogPosts/Update/" + data.record.id
                            }
                        },
                        {
                            text: l('Publish'),
                            visible: function(data) {
                                return data?.status !== blogPostStatus.Published && abp.auth.isGranted('CmsKit.BlogPosts.Publish');
                            },
                            confirmMessage: function (data) {
                                return l("BlogPostPublishConfirmationMessage", data.record.title)
                            },
                            action: function (data) {
                                blogsService
                                    .publish(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reload();
                                        abp.notify.success(l('SuccessfullyPublished'));
                                    });
                            }
                        },
                        {
                            text: l('Draft'),
                            visible: function(data) {
                                return data?.status !== blogPostStatus.Draft && abp.auth.isGranted('CmsKit.BlogPosts.Update');
                            },
                            confirmMessage: function (data) {
                                return l("BlogPostDraftConfirmationMessage", data.record.title)
                            },
                            action: function (data) {
                                blogsService
                                    .draft(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reload();
                                        abp.notify.success(l('SuccessfullySaved'));
                                    });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.BlogPosts.Delete'),
                            confirmMessage: function (data) {
                                return l("BlogPostDeletionConfirmationMessage", data.record.title)
                            },
                            action: function (data) {
                                blogsService
                                    .delete(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reload();
                                        abp.notify.success(l('SuccessfullyDeleted'));
                                    });
                            }
                        }
                    ]
                }
            },
            {
                title: l("Blog"),
                orderable: false,
                data: "blogName"
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
            },
            {
                title: l("CreationTime"),
                orderable: true,
                data: 'creationTime',
                dataFormat: "datetime"
            },
            {
                title: l("Status"),
                orderable: true,
                data: "status",
                render: function (data) {
                    return l("CmsKit.BlogPost.Status." + data);
                }
            },
        ]
    }));

    $('#CmsKitBlogPostsWrapper form.page-search-form').submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });
    
    $('#AbpContentToolbar button[name=CreateBlogPost]').on('click', function (e) {
        e.preventDefault();
        window.location.href = "BlogPosts/Create"
    });
});
