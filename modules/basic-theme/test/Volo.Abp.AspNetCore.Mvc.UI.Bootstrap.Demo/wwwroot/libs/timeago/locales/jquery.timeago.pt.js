(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Portuguese
  jQuery.timeago.settings.strings = {
     prefixAgo: "há",
     prefixFromNow: "daqui a",
     seconds: "menos de um minuto",
     minute: "cerca de um minuto",
     minutes: "%d minutos",
     hour: "cerca de uma hora",
     hours: "cerca de %d horas",
     day: "um dia",
     days: "%d dias",
     month: "cerca de um mês",
     months: "%d meses",
     year: "cerca de um ano",
     years: "%d anos"
  };
}));
