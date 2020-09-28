document.addEventListener('DOMContentLoaded', function (event) {
    setTimeout(function () {
        window.location = document
            .getElementById('redirectButton')
            .getAttribute('href');
    }, 3000);
});
