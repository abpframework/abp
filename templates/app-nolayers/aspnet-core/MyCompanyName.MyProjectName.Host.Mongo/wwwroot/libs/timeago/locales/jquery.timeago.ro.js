(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Romanian
  jQuery.timeago.settings.strings = {
    prefixAgo: "acum",
    prefixFromNow: "in timp de",
    suffixAgo: "",
    suffixFromNow: "",
    seconds: "mai putin de un minut",
    minute: "un minut",
    minutes: "%d minute",
    hour: "o ora",
    hours: "%d ore",
    day: "o zi",
    days: "%d zile",
    month: "o luna",
    months: "%d luni",
    year: "un an",
    years: "%d ani"
  };
}));

