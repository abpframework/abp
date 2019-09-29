(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Kinyarwanda
  jQuery.timeago.settings.strings = {
    prefixAgo: "hashize",
    prefixFromNow: "mu",
    suffixAgo: null,
    suffixFromNow: null,
    seconds: "amasegonda macye",
    minute: "umunota",
    minutes: "iminota %d",
    hour: "isaha",
    hours: "amasaha %d",
    day: "umunsi",
    days: "iminsi %d",
    month: "ukwezi",
    months: "amezi %d",
    year: "umwaka",
    years: "imyaka %d",
    wordSeparator: " ",
    numbers: []
  };
}));
