(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Slovak
  (function() {
  	function f(n, d, a) {
  		return a[d>=0 ? 0 : a.length===2 || n<5 ? 1 : 2];
  	}
  
  	jQuery.timeago.settings.strings = {
  		prefixAgo:     'pred',
  		prefixFromNow: 'o',
  		suffixAgo:     null,
  		suffixFromNow: null,
  		seconds: function(n, d) {return f(n, d, ['menej ako minútou', 'menej ako minútu']);},
  		minute:  function(n, d) {return f(n, d, ['minútou', 'minútu']);},
  		minutes: function(n, d) {return f(n, d, ['%d minútami', '%d minúty', '%d minút']);},
  		hour:    function(n, d) {return f(n, d, ['hodinou', 'hodinu']);},
  		hours:   function(n, d) {return f(n, d, ['%d hodinami', '%d hodiny', '%d hodín']);},
  		day:     function(n, d) {return f(n, d, ['%d dňom', '%d deň']);},
  		days:    function(n, d) {return f(n, d, ['%d dňami', '%d dni', '%d dní']);},
  		month:   function(n, d) {return f(n, d, ['%d mesiacom', '%d mesiac']);},
  		months:  function(n, d) {return f(n, d, ['%d mesiacmi', '%d mesiace', '%d mesiacov']);},
  		year:    function(n, d) {return f(n, d, ['%d rokom', '%d rok']);},
  		years:   function(n, d) {return f(n, d, ['%d rokmi', '%d roky', '%d rokov']);}
  	};
  })();
}));
