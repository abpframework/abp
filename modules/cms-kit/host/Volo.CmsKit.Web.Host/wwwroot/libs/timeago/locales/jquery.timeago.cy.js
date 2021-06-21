(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Welsh
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "yn ôl",
    suffixFromNow: "o hyn",
    seconds: "llai na munud",
    minute: "am funud",
    minutes: "%d munud",
    hour: "tua awr",
    hours: "am %d awr",
    day: "y dydd",
    days: "%d diwrnod",
    month: "tua mis",
    months: "%d mis",
    year: "am y flwyddyn",
    years: "%d blynedd",
    wordSeparator: " ",
    numbers: []
  };
}));
