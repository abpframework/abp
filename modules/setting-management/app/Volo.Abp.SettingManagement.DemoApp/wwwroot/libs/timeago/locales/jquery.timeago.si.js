(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Sinhalese (SI)
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "පෙර",
    suffixFromNow: "පසුව",
    seconds: "තත්පර කිහිපයකට",
    minute: "මිනිත්තුවකට පමණ",
    minutes: "මිනිත්තු %d කට",
    hour: "පැයක් පමණ ",
    hours: "පැය %d කට  පමණ",
    day: "දවසක ට",
    days: "දවස් %d කට ",
    month: "මාසයක් පමණ",
    months: "මාස %d කට",
    year: "වසරක් පමණ",
    years: "වසරක් %d කට පමණ"
  };
}));
