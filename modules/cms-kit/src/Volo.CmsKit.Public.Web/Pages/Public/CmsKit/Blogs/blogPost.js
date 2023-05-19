$(function () {

    let l = abp.localization.getResource("CmsKit");

    $('#deleteBlogPost').on('click', '', function (e) {
        abp.message.confirm(l("DeleteBlogPostMessage"), function (ok) {
            if (ok) {
                volo.cmsKit.public.blogs.blogPostPublic.delete(
                    $('#BlogId').val()
                ).then(function () {
                    document.location.href = "/";
                });
            }
        })
    });
});
