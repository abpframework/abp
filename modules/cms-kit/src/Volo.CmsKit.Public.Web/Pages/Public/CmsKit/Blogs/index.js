$(function () {
    let $selectAuthor = $('#AuthorSelect');
    let $authorNameSpan = $('.author-name-span');

    $selectAuthor.on('change', function () {
        let authorId = $selectAuthor.val();
        reloadPageWithQueryString({'authorId': authorId});
    });

    $authorNameSpan.click(function () {
        let authorId = $(this).data('author-id');
        reloadPageWithQueryString({'authorId': authorId});
    });

    function reloadPageWithQueryString(param) {
        window.location.href = window.location.pathname + "?" + $.param(param);
    }
});
