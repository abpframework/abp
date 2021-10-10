(function ($) {
    $(function () {
        var errorPageRedirect = function () {
            var second = 3;
            var close = setInterval(() => {
                $('#ErrorRedirectSeconds').text(`(${--second})`);
                if (second === 0) {
                    clearInterval(close);
                    $('#ErrorRedirect')[0].click();
                }
            }, 1000);
        };

        if (document.getElementById('ErrorRedirect')) {
            errorPageRedirect();
        }
    });
})(jQuery);
