(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Catalan
  jQuery.timeago.settings.strings = {
    prefixAgo: "fa",
    prefixFromNow: "d'aquí",
    suffixAgo: null,
    suffixFromNow: null,
    seconds: "menys d'un minut",
    minute: "un minut",
    minutes: "%d minuts",
    hour: "una hora",
    hours: "%d hores",
    day: "un dia",
    days: "%d dies",
    month: "un mes",
    months: "%d mesos",
    year: "un any",
    years: "%d anys",
    wordSeparator: " ",
    numbers: []
  };
}));
