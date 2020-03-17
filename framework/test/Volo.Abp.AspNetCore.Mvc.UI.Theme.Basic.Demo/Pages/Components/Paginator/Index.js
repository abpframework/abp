$(function () {
    var links = $("a.page-link");

    $.each(links, function (key, value) {
        var oldUrl = links[key].getAttribute("href");
        var value = Number(oldUrl.match(/currentPage=(\d+)&page/)[1]);
        links[key].setAttribute("href", "/Components/Paginator?currentPage=" + value);
    })
});