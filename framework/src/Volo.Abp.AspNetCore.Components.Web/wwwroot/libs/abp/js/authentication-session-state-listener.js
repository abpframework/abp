(function () {
    
    const sessionKey = 'authentication-session-id';
    
    window.addEventListener('storage', function (event) {
        
        console.log(event);
        if (event.key !== sessionKey) {
            return;
        }

        var previousSessionId = event.oldValue
        var sessionId = event.newValue;
        
        if(previousSessionId === sessionId) {
            return;
        }
        
        if(previousSessionId || !sessionId) {
            abp.utils.removeOidcUser();
            window.location.reload();
            return;
        }

        location.assign('/')
    });
}());