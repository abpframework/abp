(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Indonesian
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "yang lalu",
    suffixFromNow: "dari sekarang",
    seconds: "kurang dari semenit",
    minute: "sekitar satu menit",
    minutes: "%d menit",
    hour: "sekitar sejam",
    hours: "sekitar %d jam",
    day: "sehari",
    days: "%d hari",
    month: "sekitar sebulan",
    months: "%d bulan",
    year: "sekitar setahun",
    years: "%d tahun"
  };
}));

