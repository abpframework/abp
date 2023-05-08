$(function () {
    debugger
    var hash = window.location.hash;
    if(hash === '#edit-profile'){
        console.log("asdasdsa")
        $('#all-posts-tab').removeClass('active');
        $('#all-posts').removeClass('show').removeClass('active');
        $('#edit-profile-tab').addClass('active');
        $('#edit-profile').addClass('show').addClass('active');
        window.location.hash = '';
    }
});