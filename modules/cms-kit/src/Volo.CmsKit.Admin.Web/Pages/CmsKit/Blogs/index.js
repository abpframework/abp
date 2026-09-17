
$(function () {
    var l = abp.localization.getResource("CmsKit");

    var createModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Blogs/CreateModal", modalClass: 'createBlog' });
    var updateModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Blogs/UpdateModal", modalClass: 'updateBlog' });
    var featuresModal = new abp.ModalManager(abp.appPath + "CmsKit/Blogs/FeaturesModal");


    var deleteBlogModal = new abp.ModalManager(abp.appPath + 'CmsKit/Blogs/DeleteBlogModal');

    deleteBlogModal.onResult(function(){
        abp.notify.success(l('DeletedSuccessfully'));
    });

    deleteBlogModal.onOpen(function () {
        var $form = deleteBlogModal.getForm();
        $form.find('#assign').click(function () {
            $form.find('#Blog_AssignToBlogId').show();
            $form.find('[type=submit]').attr("disabled","disabled")
        })
        $form.find('#deleteAll').click(function () {
            $form.find('#Blog_AssignToBlogId').hide();
            $form.find('#Blog_AssignToBlogId').val("");
            $form.find('[type=submit]').removeAttr("disabled");
        })

        $("#Blog_AssignToBlogId").on("change", function () {
            var val = $(this).val();
            if(val === ''){
                $form.find('[type=submit]').attr("disabled","disabled")
            }else{
                $form.find('[type=submit]').removeAttr("disabled");
            }
        })
    })
    
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
                            action: function (data) {
                                deleteBlogModal.open({
                                    id: data.record.id
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
        dataTable.ajax.reloadEx();
    });

    updateModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });

    deleteBlogModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });
});