(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  //Uzbek
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: "keyin",
    suffixAgo: "avval",
    suffixFromNow: null,
    seconds: "bir necha soniya",
    minute: "1 daqiqa",
    minutes: function(value) { return "%d daqiqa"; },
    hour: "1 soat",
    hours: function(value) { return "%d soat"; },
    day: "1 kun",
    days: function(value) { return "%d kun"; },
    month: "1 oy",
    months: function(value) { return "%d oy"; },
    year: "1 yil",
    years: function(value) { return "%d yil"; },
    wordSeparator: " "
  };
}));
