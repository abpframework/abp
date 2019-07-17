(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Dutch
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: "over",
    suffixAgo: "geleden",
    suffixFromNow: null,
    seconds: "minder dan een minuut",
    minute: "ongeveer een minuut",
    minutes: "%d minuten",
    hour: "ongeveer een uur",
    hours: "ongeveer %d uur",
    day: "een dag",
    days: "%d dagen",
    month: "ongeveer een maand",
    months: "%d maanden",
    year: "ongeveer een jaar",
    years: "%d jaar",
    wordSeparator: " ",
    numbers: []
  };
}));
