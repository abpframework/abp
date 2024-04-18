(function () {
    
    const stateKey = 'authentication-state-id';
    
    window.addEventListener('storage', function (event) {
        
        if (event.key !== stateKey) {
            return;
        }

        var previousState = event.oldValue
        var state = event.newValue;
        
        if(previousState === state) {
            return;
        }
        
        if(previousState || !state) {
            abp.utils.removeOidcUser();
            window.location.reload();
            return;
        }

        location.assign('/')
    });
}());