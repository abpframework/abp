(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Japanese
  jQuery.timeago.settings.strings = {
    prefixAgo: "",
    prefixFromNow: "今から",
    suffixAgo: "前",
    suffixFromNow: "後",
    seconds: "1 分未満",
    minute: "約 1 分",
    minutes: "%d 分",
    hour: "約 1 時間",
    hours: "約 %d 時間",
    day: "約 1 日",
    days: "約 %d 日",
    month: "約 1 ヶ月",
    months: "約 %d ヶ月",
    year: "約 1 年",
    years: "約 %d 年",
    wordSeparator: ""
  };
}));
