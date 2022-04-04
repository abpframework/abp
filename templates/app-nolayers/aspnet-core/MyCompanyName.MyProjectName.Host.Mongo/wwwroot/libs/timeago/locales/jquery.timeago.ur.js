(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Urdu 
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "پہلے",
    suffixFromNow: "اب سے",
    seconds: "کچھ سیکنڈز",
    minute: "تقریباً ایک منٹ",
    minutes: "%d منٹ",
    hour: "تقریباً ایک گھنٹہ",
    hours: "تقریباً  %d گھنٹے",
    day: "ایک دن",
    days: "%d دن",
    month: "تقریباً ایک مہینہ",
    months: "%d مہینے",
    year: "تقریباً ایک سال",
    years: "%d سال",
    wordSeparator: " ",
    numbers: []
  };
}));
