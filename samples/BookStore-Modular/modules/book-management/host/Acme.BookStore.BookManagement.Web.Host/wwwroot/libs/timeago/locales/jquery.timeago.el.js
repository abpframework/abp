(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Greek
  jQuery.timeago.settings.strings = {
    prefixAgo: "πριν",
    prefixFromNow: "σε",
    suffixAgo: "",
    suffixFromNow: "",
    seconds: "λιγότερο από ένα λεπτό",
    minute: "περίπου ένα λεπτό",
    minutes: "%d λεπτά",
    hour: "περίπου μία ώρα",
    hours: "περίπου %d ώρες",
    day: "μία μέρα",
    days: "%d μέρες",
    month: "περίπου ένα μήνα",
    months: "%d μήνες",
    year: "περίπου ένα χρόνο",
    years: "%d χρόνια"
  };
}));
