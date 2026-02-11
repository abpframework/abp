$(function () {
    var $enlargeImage = $("article img");

    if (!$enlargeImage.length) {
        return;
    }

    $enlargeImage.addClass("docs-enlarge-image");

    var $modal = $("#image-modal");

    if (!$modal.length) {
        $modal = $("<div class='docs-image-modal'><span class='close'>&times;</span><img class='docs-image-modal-content'/></div>");
        $("body").append($modal);
    }

    var modalImg = $modal.find("img");
    $enlargeImage.click(function () {
        var width = $(this).width();
        var height = $(this).height();
        modalImg.attr("src", $(this).findWithSelf("img").attr("src"));
        modalImg.css("min-width", width + "px");
        modalImg.css("min-height", height + "px");
        $modal.addClass("show");
    });

    $modal.find(".close").click(modalClose);

    $modal.click(modalClose);

    function modalClose() {
        $modal.removeClass("show");
    }
});