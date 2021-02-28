(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // French shortened
  jQuery.timeago.settings.strings = {
     prefixAgo: "il y a",
     prefixFromNow: "d'ici",
     seconds: "moins d'une minute",
     minute: "une minute",
     minutes: "%d minutes",
     hour: "une heure",
     hours: "%d heures",
     day: "un jour",
     days: "%d jours",
     month: "un mois",
     months: "%d mois",
     year: "un an",
     years: "%d ans"
  };
}));
