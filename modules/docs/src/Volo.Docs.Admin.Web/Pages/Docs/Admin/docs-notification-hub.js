"use strict";

var hubName = "document-notification-hub";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/" + hubName)
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

var docsNotificationManager = $.extend({}, docsNotificationManager, {
    messageListeners: {
        notificationProcessMessages: []
    }
});

connection.on("receiveNotificationMessage", function (message) {
    if (!docsNotificationManager.messageListeners.notificationProcessMessages) {
        return;
    }

    docsNotificationManager.messageListeners.notificationProcessMessages.forEach(function (fn) {
        fn({
            message: message
        });
    });
});

var showNotification = function(notification) {
    abp.notify.info(notification.message);
};

docsNotificationManager
    .messageListeners
    .notificationProcessMessages
    .push(showNotification);

connection.start().then(function () {
    abp.log.debug(hubName + " connected.");
}).catch(function (err) {
    abp.log.error(err.toString());
});