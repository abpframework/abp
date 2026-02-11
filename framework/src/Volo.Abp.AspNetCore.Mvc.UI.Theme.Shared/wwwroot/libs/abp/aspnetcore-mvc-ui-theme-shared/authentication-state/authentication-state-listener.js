(function () {

    const stateKey = 'authentication-state-id';

    window.addEventListener('load', function () {
        if (!abp || !abp.currentUser) {
            return;
        }

        if (!abp.currentUser.isAuthenticated) {
            localStorage.removeItem(stateKey);
        } else {
            localStorage.setItem(stateKey, abp.currentUser.id);
        }

        window.addEventListener('storage', function (event) {

            if (event.key !== stateKey || event.oldValue === event.newValue) {
                return;
            }

            if (event.oldValue || !event.newValue) {
                window.location.reload();
            } else {
                location.assign('/')
            }
        });
    });

}());