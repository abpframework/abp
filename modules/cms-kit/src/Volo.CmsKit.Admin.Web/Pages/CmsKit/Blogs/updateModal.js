var abp = abp || {};
$(function () {
    abp.modals.updateBlog = function () {
        var initModal = function (publicApi, args) {

            var $slug = $('#ViewModel_Slug');

            $slug.on('change', function () {
                reflectUrlChanges();
            });

            function reflectUrlChanges() {

                var title = $slug.val();

                var slugified = slugify(title);

                if (slugified != title) {

                    $slug.val(slugified, {
                        lower: true
                    });
                }
            }
        }
        return {
            initModal: initModal
        };
    };
});