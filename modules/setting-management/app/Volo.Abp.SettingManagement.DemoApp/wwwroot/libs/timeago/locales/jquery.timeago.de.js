(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // German
  jQuery.timeago.settings.strings = {
    prefixAgo: "vor",
    prefixFromNow: "in",
    suffixAgo: "",
    suffixFromNow: "",
    seconds: "wenigen Sekunden",
    minute: "etwa einer Minute",
    minutes: "%d Minuten",
    hour: "etwa einer Stunde",
    hours: "%d Stunden",
    day: "etwa einem Tag",
    days: "%d Tagen",
    month: "etwa einem Monat",
    months: "%d Monaten",
    year: "etwa einem Jahr",
    years: "%d Jahren"
  };
}));
