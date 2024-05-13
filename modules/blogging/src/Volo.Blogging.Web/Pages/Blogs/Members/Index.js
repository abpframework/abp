$(function () {
    if (window.location.hash === '#edit-profile'){
        $('#all-posts-tab').removeClass('active');
        $('#all-posts').removeClass('show').removeClass('active');
        $('#edit-profile-tab').addClass('active');
        $('#edit-profile').addClass('show').addClass('active');
        window.location.hash = '';
    }
});
