(function ($) {
    //TODO:gterdem There should be an option to set the redirect delay here
    setTimeout(function () {
        window.location = $('.redirectButton').attr('href');
    }, 3000)
})(JQuery)
