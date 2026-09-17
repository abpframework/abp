var abp = abp || {};
(function ($) {
    var $all = $("#grantAllresourcePermissions");
    var $items = $("#permissionList input[type='checkbox']").not("#grantAllresourcePermissions");
    $all.on("change", function () {
        $items.prop("checked", this.checked);
    });
    $items.on("change", function () {
        $all.prop("checked", $items.length === $items.filter(":checked").length);
    });
})(jQuery);
