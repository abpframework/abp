(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Hebrew
  jQuery.timeago.settings.strings = {
    prefixAgo: "לפני",
    prefixFromNow: "עוד",
    seconds: "פחות מדקה",
    minute: "דקה",
    minutes: "%d דקות",
    hour: "שעה",
    hours: function(number){return (number===2) ? "שעתיים" : "%d שעות";},
    day: "יום",
    days: function(number){return (number===2) ? "יומיים" : "%d ימים";},
    month: "חודש",
    months: function(number){return (number===2) ? "חודשיים" : "%d חודשים";},
    year: "שנה",
    years: function(number){return (number===2) ? "שנתיים" : "%d שנים";}
  };
}));
