$(function () {
    abp.log.debug('Index.js initialized!');

    const eventBus = new abp.EventBus();
    eventBus.on('someEvent', () => console.log('someEvent triggered'));

    eventBus.trigger('someEvent');
});
