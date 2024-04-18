(function () {

    const stateKey = 'authentication-state-id';

    window.addEventListener('storage', function (event) {
        if (event.key !== stateKey || event.oldValue === event.newValue) {
            return;
        }
        
        if (event.oldValue || !event.newValue) {
            abp.utils.removeOidcUser();
            window.location.reload();
        } else {
            location.assign('/')
        }
    });
}());