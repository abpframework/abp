document.addEventListener("DOMContentLoaded", function (event) {
    setTimeout(function () {
        var redirectButton = document.getElementById("redirectButton");
        if (!redirectButton) {
            return;
        }
        var clientName = redirectButton.getAttribute("cname");
        if (clientName) {
            window.clientName = clientName;
        }
        var href = redirectButton.getAttribute("href");
        if (!href) {
            return;
        }
        window.location = href;
    }, 3000);
});
