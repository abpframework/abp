var abp = abp || {};
$(function () {
    abp.modals.createBlog = function () {
        var initModal = function (publicApi, args) {

            var $name = $('#ViewModel_Name');
            var $slug = $('#ViewModel_Slug');

            var urlEdited = false;

            $name.on('change paste keyup', function () {
                reflectUrlChanges();
            });

            $slug.on('change', function () {
                reflectUrlChanges();
            });

            function reflectUrlChanges() {
                var title = $name.val();

                if (urlEdited) {
                    title = $slug.val();
                }

                var slugified = slugify(title, {
                    lower: true
                });

                if (slugified != $slug.val()) {
                    reflectedChange = true;

                    $slug.val(slugified);

                    reflectedChange = false;
                }
            }

            $slug.change(function () {
                urlEdited = true;
            });
        }
        return {
            initModal: initModal
        };
    };
});