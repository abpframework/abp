(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Persian
  // Use DIR attribute for RTL text in Persian Language for ABBR tag .
  // By MB.seifollahi@gmail.com
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "پیش",
    suffixFromNow: "از حال",
    seconds: "کمتر از یک دقیقه",
    minute: "حدود یک دقیقه",
    minutes: "%d دقیقه",
    hour: "حدود یک ساعت",
    hours: "حدود %d ساعت",
    day: "یک روز",
    days: "%d روز",
    month: "حدود یک ماه",
    months: "%d ماه",
    year: "حدود یک سال",
    years: "%d سال",
    wordSeparator: " ",
    numbers: ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹']
  };
}));
