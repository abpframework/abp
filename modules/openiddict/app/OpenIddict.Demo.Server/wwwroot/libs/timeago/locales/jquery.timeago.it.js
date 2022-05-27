(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Italian
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: "fra",
    suffixAgo: "fa",
    suffixFromNow: null,
    seconds: "meno di un minuto",
    minute: "circa un minuto",
    minutes: "%d minuti",
    hour: "circa un'ora",
    hours: "circa %d ore",
    day: "un giorno",
    days: "%d giorni",
    month: "circa un mese",
    months: "%d mesi",
    year: "circa un anno",
    years: "%d anni"
  };
}));
