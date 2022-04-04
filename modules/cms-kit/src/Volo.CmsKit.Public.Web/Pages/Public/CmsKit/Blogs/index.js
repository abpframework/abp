$(function () {
    var $selectAuthor = $('#AuthorSelect');
    var $authorNameSpan = $('.author-name-span');

    $selectAuthor.on('change', function () {
        var authorId = $selectAuthor.val();
        reloadPageWithQueryString({'authorId': authorId});
    });

    $authorNameSpan.click(function () {
        var authorId = $(this).data('author-id');
        reloadPageWithQueryString({'authorId': authorId});
    });

    function reloadPageWithQueryString(param) {
        window.location.href = window.location.pathname + "?" + $.param(param);
    }
});
