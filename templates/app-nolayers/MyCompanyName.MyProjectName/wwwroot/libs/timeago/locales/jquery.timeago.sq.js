(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Albanian SQ
  jQuery.timeago.settings.strings = {
    suffixAgo: "më parë",
    suffixFromNow: "tani",
    seconds: "më pak se një minutë",
    minute: "rreth një minutë",
    minutes: "%d minuta",
    hour: "rreth një orë",
    hours: "rreth %d orë",
    day: "një ditë",
    days: "%d ditë",
    month: "rreth një muaj",
    months: "%d muaj",
    year: "rreth një vit",
    years: "%d vjet"
  };
}));
