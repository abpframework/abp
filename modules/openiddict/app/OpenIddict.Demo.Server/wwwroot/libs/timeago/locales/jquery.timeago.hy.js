(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Armenian
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "առաջ",
    suffixFromNow: "հետո",
    seconds: "վայրկյաններ",
    minute: "մեկ րոպե",
    minutes: "%d րոպե",
    hour: "մեկ ժամ",
    hours: "%d ժամ",
    day: "մեկ օր",
    days: "%d օր",
    month: "մեկ ամիս",
    months: "%d ամիս",
    year: "մեկ տարի",
    years: "%d տարի"
  };
}));
