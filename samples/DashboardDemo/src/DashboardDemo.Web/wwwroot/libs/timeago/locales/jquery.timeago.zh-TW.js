(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Traditional Chinese, zh-tw
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "之前",
    suffixFromNow: "之後",
    seconds: "不到1分鐘",
    minute: "大約1分鐘",
    minutes: "%d分鐘",
    hour: "大約1小時",
    hours: "%d小時",
    day: "大約1天",
    days: "%d天",
    month: "大約1個月",
    months: "%d個月",
    year: "大約1年",
    years: "%d年",
    numbers: [],
    wordSeparator: ""
  };
}));
