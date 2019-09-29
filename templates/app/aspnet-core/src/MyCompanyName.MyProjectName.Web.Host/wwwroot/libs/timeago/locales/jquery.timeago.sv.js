(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Swedish
  jQuery.timeago.settings.strings = {
    prefixAgo: "för",
    prefixFromNow: "om",
    suffixAgo: "sedan",
    suffixFromNow: "",
    seconds: "mindre än en minut",
    minute: "ungefär en minut",
    minutes: "%d minuter",
    hour: "ungefär en timme",
    hours: "ungefär %d timmar",
    day: "en dag",
    days: "%d dagar",
    month: "ungefär en månad",
    months: "%d månader",
    year: "ungefär ett år",
    years: "%d år"
  };
}));
