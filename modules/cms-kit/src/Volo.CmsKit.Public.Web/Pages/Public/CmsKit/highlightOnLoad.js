$(function () {

    document.querySelectorAll('code').forEach(block => {
        $(block).addClass('hljs'); // Put in gray box even language is not supported
        hljs.highlightBlock(block);
    });

})