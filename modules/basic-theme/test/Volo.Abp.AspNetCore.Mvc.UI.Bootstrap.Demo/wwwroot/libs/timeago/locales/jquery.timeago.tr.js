(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Turkish
  jQuery.timeago.settings.strings = {
    suffixAgo: 'önce',
    suffixFromNow: null,
    seconds: 'birkaç saniye',
    minute: '1 dakika',
    minutes: '%d dakika',
    hour: '1 saat',
    hours: '%d saat',
    day: '1 gün',
    days: '%d gün',
    month: '1 ay',
    months: '%d ay',
    year: '1 yıl',
    years: '%d yıl'
  };
}));
