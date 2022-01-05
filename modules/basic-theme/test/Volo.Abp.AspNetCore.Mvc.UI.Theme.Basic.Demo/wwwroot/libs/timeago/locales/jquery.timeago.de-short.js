(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // German shortened
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "",
    suffixFromNow: "",
    seconds: "s",
    minute: "1m",
    minutes: "%dm",
    hour: "1h",
    hours: "%dh",
    day: "1T.",
    days: "%dT.",
    month: "1Mt.",
    months: "%dMt.",
    year: "1J.",
    years: "%dJ.",
    wordSeparator: " ",
    numbers: []
  };
}));
