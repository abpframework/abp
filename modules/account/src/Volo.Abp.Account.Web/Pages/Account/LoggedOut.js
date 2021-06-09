document.addEventListener('DOMContentLoaded', function (event) {
    setTimeout(function () {
        window.clientName = document.getElementById("redirectButton").getAttribute("cname");
        window.location = document.getElementById('redirectButton').getAttribute('href');
    }, 3000);
});
