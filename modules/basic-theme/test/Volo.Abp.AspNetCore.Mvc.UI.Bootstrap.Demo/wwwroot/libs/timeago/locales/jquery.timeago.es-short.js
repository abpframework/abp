(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Spanish shortened
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "",
    suffixFromNow: "",
    seconds: "1m",
    minute: "1m",
    minutes: "%dm",
    hour: "1h",
    hours: "%dh",
    day: "1d",
    days: "%dd",
    month: "1me",
    months: "%dme",
    year: "1a",
    years: "%da",
    wordSeparator: " ",
    numbers: []
  };
}));

