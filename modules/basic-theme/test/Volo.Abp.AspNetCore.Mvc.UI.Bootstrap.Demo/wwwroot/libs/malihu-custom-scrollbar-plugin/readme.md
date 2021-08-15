malihu custom scrollbar plugin
================================

Highly customizable custom scrollbar jQuery plugin ([Demo](http://manos.malihu.gr/repository/custom-scrollbar/demo/examples/complete_examples.html)). Features include: 

* Vertical and/or horizontal scrollbar(s)  
* Adjustable scrolling momentum 
* Mouse-wheel, keyboard and touch support 
* Ready-to-use themes and customization via CSS 
* RTL direction support 
* Option parameters for full control of scrollbar functionality 
* Methods for triggering actions like scroll-to, update, destroy etc. 
* User-defined callbacks 
* Selectable/searchable content

**[Plugin homepage and documentation](http://manos.malihu.gr/jquery-custom-content-scroller/)** ([Changelog](http://manos.malihu.gr/jquery-custom-content-scroller/2/)) 

#### Installation

npm: `npm install malihu-custom-scrollbar-plugin` 

Bower: `bower install malihu-custom-scrollbar-plugin` 

[Manual](http://manos.malihu.gr/jquery-custom-content-scroller/#get-started-section) 

#### Usage 

Manual: `$(selector).mCustomScrollbar();` 

[Browserify](http://browserify.org/): 

    var $ = require('jquery');
    require('malihu-custom-scrollbar-plugin')($);

[webpack](https://webpack.github.io/): 

    npm install imports-loader
	npm install jquery-mousewheel
	npm install malihu-custom-scrollbar-plugin

	module.exports = {
		module: {
			loaders: [
				{ test: /jquery-mousewheel/, loader: "imports?define=>false&this=>window" },
				{ test: /malihu-custom-scrollbar-plugin/, loader: "imports?define=>false&this=>window" }
			]
		}
	};

	var $ = require('jquery');
	require("jquery-mousewheel")($);
    require('malihu-custom-scrollbar-plugin')($);


Requirements
-------------------------

jQuery version **1.6.0** or higher

Browser compatibility
-------------------------

* Internet Explorer 8+ 
* Firefox 
* Chrome 
* Opera 
* Safari  
* iOS 
* Android 
* Windows Phone

License 
-------------------------

MIT License (MIT)

http://opensource.org/licenses/MIT

Donate 
-------------------------

https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UYJ5G65M6ZA28