$(function () {
    abp.log.debug('Index.js initialized!');
    var eventBus = new abp.EventBus();
    eventBus.on('someEvent', function () { return console.log('someEvent triggered'); });
    eventBus.trigger('someEvent');
});
//# sourceMappingURL=Index.js.map