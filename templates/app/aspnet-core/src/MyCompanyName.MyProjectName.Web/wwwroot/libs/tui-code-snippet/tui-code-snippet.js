/*!
 * TOAST UI Code Snippet
 * @version 2.3.2
 * @author NHN. FE Development Lab <dl_javascript@nhn.com>
 * @license MIT
 */
(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if(typeof define === 'function' && define.amd)
		define([], factory);
	else if(typeof exports === 'object')
		exports["util"] = factory();
	else
		root["tui"] = root["tui"] || {}, root["tui"]["util"] = factory();
})(window, function() {
return /******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "dist";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 36);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is an instance of Array or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is an instance of Array or not.
 * If the given variable is an instance of Array, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is array instance?
 * @memberof module:type
 */
function isArray(obj) {
  return obj instanceof Array;
}

module.exports = isArray;


/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Execute the provided callback once for each property of object(or element of array) which actually exist.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isArray = __webpack_require__(0);
var forEachArray = __webpack_require__(3);
var forEachOwnProperties = __webpack_require__(5);

/**
 * @module collection
 */

/**
 * Execute the provided callback once for each property of object(or element of array) which actually exist.
 * If the object is Array-like object(ex-arguments object), It needs to transform to Array.(see 'ex2' of example).
 * If the callback function returns false, the loop will be stopped.
 * Callback function(iteratee) is invoked with three arguments:
 *  1) The value of the property(or The value of the element)
 *  2) The name of the property(or The index of the element)
 *  3) The object being traversed
 * @param {Object} obj The object that will be traversed
 * @param {function} iteratee Callback function
 * @param {Object} [context] Context(this) of callback function
 * @memberof module:collection
 * @example
 * var forEach = require('tui-code-snippet/collection/forEach'); // node, commonjs
 *
 * var sum = 0;
 *
 * forEach([1,2,3], function(value){
 *     sum += value;
 * });
 * alert(sum); // 6
 *
 * // In case of Array-like object
 * var array = Array.prototype.slice.call(arrayLike); // change to array
 * forEach(array, function(value){
 *     sum += value;
 * });
 */
function forEach(obj, iteratee, context) {
  if (isArray(obj)) {
    forEachArray(obj, iteratee, context);
  } else {
    forEachOwnProperties(obj, iteratee, context);
  }
}

module.exports = forEach;


/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is undefined or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is undefined or not.
 * If the given variable is undefined, returns true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is undefined?
 * @memberof module:type
 */
function isUndefined(obj) {
  return obj === undefined; // eslint-disable-line no-undefined
}

module.exports = isUndefined;


/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Execute the provided callback once for each element present in the array(or Array-like object) in ascending order.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Execute the provided callback once for each element present
 * in the array(or Array-like object) in ascending order.
 * If the callback function returns false, the loop will be stopped.
 * Callback function(iteratee) is invoked with three arguments:
 *  1) The value of the element
 *  2) The index of the element
 *  3) The array(or Array-like object) being traversed
 * @param {Array|Arguments|NodeList} arr The array(or Array-like object) that will be traversed
 * @param {function} iteratee Callback function
 * @param {Object} [context] Context(this) of callback function
 * @memberof module:collection
 * @example
 * var forEachArray = require('tui-code-snippet/collection/forEachArray'); // node, commonjs
 *
 * var sum = 0;
 *
 * forEachArray([1,2,3], function(value){
 *     sum += value;
 * });
 * alert(sum); // 6
 */
function forEachArray(arr, iteratee, context) {
  var index = 0;
  var len = arr.length;

  context = context || null;

  for (; index < len; index += 1) {
    if (iteratee.call(context, arr[index], index, arr) === false) {
      break;
    }
  }
}

module.exports = forEachArray;


/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* eslint-disable complexity */
/**
 * @fileoverview Returns the first index at which a given element can be found in the array.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isArray = __webpack_require__(0);

/**
 * @module array
 */

/**
 * Returns the first index at which a given element can be found in the array
 * from start index(default 0), or -1 if it is not present.
 * It compares searchElement to elements of the Array using strict equality
 * (the same method used by the ===, or triple-equals, operator).
 * @param {*} searchElement Element to locate in the array
 * @param {Array} array Array that will be traversed.
 * @param {number} startIndex Start index in array for searching (default 0)
 * @returns {number} the First index at which a given element, or -1 if it is not present
 * @memberof module:array
 * @example
 * var inArray = require('tui-code-snippet/array/inArray'); // node, commonjs
 *
 * var arr = ['one', 'two', 'three', 'four'];
 * var idx1 = inArray('one', arr, 3); // -1
 * var idx2 = inArray('one', arr); // 0
 */
function inArray(searchElement, array, startIndex) {
  var i;
  var length;
  startIndex = startIndex || 0;

  if (!isArray(array)) {
    return -1;
  }

  if (Array.prototype.indexOf) {
    return Array.prototype.indexOf.call(array, searchElement, startIndex);
  }

  length = array.length;
  for (i = startIndex; startIndex >= 0 && i < length; i += 1) {
    if (array[i] === searchElement) {
      return i;
    }
  }

  return -1;
}

module.exports = inArray;


/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Execute the provided callback once for each property of object which actually exist.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Execute the provided callback once for each property of object which actually exist.
 * If the callback function returns false, the loop will be stopped.
 * Callback function(iteratee) is invoked with three arguments:
 *  1) The value of the property
 *  2) The name of the property
 *  3) The object being traversed
 * @param {Object} obj The object that will be traversed
 * @param {function} iteratee  Callback function
 * @param {Object} [context] Context(this) of callback function
 * @memberof module:collection
 * @example
 * var forEachOwnProperties = require('tui-code-snippet/collection/forEachOwnProperties'); // node, commonjs
 *
 * var sum = 0;
 *
 * forEachOwnProperties({a:1,b:2,c:3}, function(value){
 *     sum += value;
 * });
 * alert(sum); // 6
 */
function forEachOwnProperties(obj, iteratee, context) {
  var key;

  context = context || null;

  for (key in obj) {
    if (obj.hasOwnProperty(key)) {
      if (iteratee.call(context, obj[key], key, obj) === false) {
        break;
      }
    }
  }
}

module.exports = forEachOwnProperties;


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Extend the target object from other objects.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * @module object
 */

/**
 * Extend the target object from other objects.
 * @param {object} target - Object that will be extended
 * @param {...object} objects - Objects as sources
 * @returns {object} Extended object
 * @memberof module:object
 */
function extend(target, objects) { // eslint-disable-line no-unused-vars
  var hasOwnProp = Object.prototype.hasOwnProperty;
  var source, prop, i, len;

  for (i = 1, len = arguments.length; i < len; i += 1) {
    source = arguments[i];
    for (prop in source) {
      if (hasOwnProp.call(source, prop)) {
        target[prop] = source[prop];
      }
    }
  }

  return target;
}

module.exports = extend;


/***/ }),
/* 7 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is an object or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is an object or not.
 * If the given variable is an object, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is object?
 * @memberof module:type
 */
function isObject(obj) {
  return obj === Object(obj);
}

module.exports = isObject;


/***/ }),
/* 8 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a string or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a string or not.
 * If the given variable is a string, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is string?
 * @memberof module:type
 */
function isString(obj) {
  return typeof obj === 'string' || obj instanceof String;
}

module.exports = isString;


/***/ }),
/* 9 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is existing or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isUndefined = __webpack_require__(2);
var isNull = __webpack_require__(11);

/**
 * Check whether the given variable is existing or not.
 * If the given variable is not null and not undefined, returns true.
 * @param {*} param - Target for checking
 * @returns {boolean} Is existy?
 * @memberof module:type
 * @example
 * var isExisty = require('tui-code-snippet/type/isExisty'); // node, commonjs
 *
 * isExisty(''); //true
 * isExisty(0); //true
 * isExisty([]); //true
 * isExisty({}); //true
 * isExisty(null); //false
 * isExisty(undefined); //false
*/
function isExisty(param) {
  return !isUndefined(param) && !isNull(param);
}

module.exports = isExisty;


/***/ }),
/* 10 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Get HTML element's design classes.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isUndefined = __webpack_require__(2);

/**
 * Get HTML element's design classes.
 * @param {(HTMLElement|SVGElement)} element target element
 * @returns {string} element css class name
 * @memberof module:domUtil
 */
function getClass(element) {
  if (!element || !element.className) {
    return '';
  }

  if (isUndefined(element.className.baseVal)) {
    return element.className;
  }

  return element.className.baseVal;
}

module.exports = getClass;


/***/ }),
/* 11 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is null or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is null or not.
 * If the given variable(arguments[0]) is null, returns true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is null?
 * @memberof module:type
 */
function isNull(obj) {
  return obj === null;
}

module.exports = isNull;


/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a function or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a function or not.
 * If the given variable is a function, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is function?
 * @memberof module:type
 */
function isFunction(obj) {
  return obj instanceof Function;
}

module.exports = isFunction;


/***/ }),
/* 13 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* eslint-disable complexity */
/**
 * @fileoverview Check whether the given variable is empty(null, undefined, or empty array, empty object) or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isString = __webpack_require__(8);
var isExisty = __webpack_require__(9);
var isArray = __webpack_require__(0);
var isArguments = __webpack_require__(20);
var isObject = __webpack_require__(7);
var isFunction = __webpack_require__(12);

/**
 * Check whether given argument is empty string
 * @param {*} obj - Target for checking
 * @returns {boolean} whether given argument is empty string
 * @private
 */
function _isEmptyString(obj) {
  return isString(obj) && obj === '';
}

/**
 * Check whether given argument has own property
 * @param {Object} obj - Target for checking
 * @returns {boolean} - whether given argument has own property
 * @private
 */
function _hasOwnProperty(obj) {
  var key;
  for (key in obj) {
    if (obj.hasOwnProperty(key)) {
      return true;
    }
  }

  return false;
}

/**
 * Check whether the given variable is empty(null, undefined, or empty array, empty object) or not.
 *  If the given variables is empty, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is empty?
 * @memberof module:type
 */
function isEmpty(obj) {
  if (!isExisty(obj) || _isEmptyString(obj)) {
    return true;
  }

  if (isArray(obj) || isArguments(obj)) {
    return obj.length === 0;
  }

  if (isObject(obj) && !isFunction(obj)) {
    return !_hasOwnProperty(obj);
  }

  return true;
}

module.exports = isEmpty;


/***/ }),
/* 14 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Transform the Array-like object to Array.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEachArray = __webpack_require__(3);

/**
 * Transform the Array-like object to Array.
 * In low IE (below 8), Array.prototype.slice.call is not perfect. So, try-catch statement is used.
 * @param {*} arrayLike Array-like object
 * @returns {Array} Array
 * @memberof module:collection
 * @example
 * var toArray = require('tui-code-snippet/collection/toArray'); // node, commonjs
 *
 * var arrayLike = {
 *     0: 'one',
 *     1: 'two',
 *     2: 'three',
 *     3: 'four',
 *     length: 4
 * };
 * var result = toArray(arrayLike);
 *
 * alert(result instanceof Array); // true
 * alert(result); // one,two,three,four
 */
function toArray(arrayLike) {
  var arr;
  try {
    arr = Array.prototype.slice.call(arrayLike);
  } catch (e) {
    arr = [];
    forEachArray(arrayLike, function(value) {
      arr.push(value);
    });
  }

  return arr;
}

module.exports = toArray;


/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Unbind DOM events
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isString = __webpack_require__(8);
var forEach = __webpack_require__(1);

var safeEvent = __webpack_require__(24);

/**
 * Unbind DOM events
 * If a handler function is not passed, remove all events of that type.
 * @param {HTMLElement} element - element to unbind events
 * @param {(string|object)} types - Space splitted events names or eventName:handler object
 * @param {function} [handler] - handler function
 * @memberof module:domEvent
 * @example
 * // Following the example of domEvent#on
 * 
 * // Unbind one event from an element.
 * off(div, 'click', toggle);
 * 
 * // Unbind multiple events with a same handler from multiple elements at once.
 * // Use event names splitted by a space.
 * off(element, 'mouseenter mouseleave', changeColor);
 * 
 * // Unbind multiple events with different handlers from an element at once.
 * // Use an object which of key is an event name and value is a handler function.
 * off(div, {
 *   keydown: highlight,
 *   keyup: dehighlight
 * });
 * 
 * // Unbind events without handlers.
 * off(div, 'drag');
 */
function off(element, types, handler) {
  if (isString(types)) {
    forEach(types.split(/\s+/g), function(type) {
      unbindEvent(element, type, handler);
    });

    return;
  }

  forEach(types, function(func, type) {
    unbindEvent(element, type, func);
  });
}

/**
 * Unbind DOM events
 * If a handler function is not passed, remove all events of that type.
 * @param {HTMLElement} element - element to unbind events
 * @param {string} type - events name
 * @param {function} [handler] - handler function
 * @private
 */
function unbindEvent(element, type, handler) {
  var events = safeEvent(element, type);
  var index;

  if (!handler) {
    forEach(events, function(item) {
      removeHandler(element, type, item.wrappedHandler);
    });
    events.splice(0, events.length);
  } else {
    forEach(events, function(item, idx) {
      if (handler === item.handler) {
        removeHandler(element, type, item.wrappedHandler);
        index = idx;

        return false;
      }

      return true;
    });
    events.splice(index, 1);
  }
}

/**
 * Remove an event handler
 * @param {HTMLElement} element - An element to remove an event
 * @param {string} type - event type
 * @param {function} handler - event handler
 * @private
 */
function removeHandler(element, type, handler) {
  if ('removeEventListener' in element) {
    element.removeEventListener(type, handler);
  } else if ('detachEvent' in element) {
    element.detachEvent('on' + type, handler);
  }
}

module.exports = off;


/***/ }),
/* 16 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Bind DOM events
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isString = __webpack_require__(8);
var forEach = __webpack_require__(1);

var safeEvent = __webpack_require__(24);

/**
 * Bind DOM events.
 * @param {HTMLElement} element - element to bind events
 * @param {(string|object)} types - Space splitted events names or eventName:handler object
 * @param {(function|object)} handler - handler function or context for handler method
 * @param {object} [context] context - context for handler method.
 * @memberof module:domEvent
 * @example
 * var div = document.querySelector('div');
 * 
 * // Bind one event to an element.
 * on(div, 'click', toggle);
 * 
 * // Bind multiple events with a same handler to multiple elements at once.
 * // Use event names splitted by a space.
 * on(div, 'mouseenter mouseleave', changeColor);
 * 
 * // Bind multiple events with different handlers to an element at once.
 * // Use an object which of key is an event name and value is a handler function.
 * on(div, {
 *   keydown: highlight,
 *   keyup: dehighlight
 * });
 * 
 * // Set a context for handler method.
 * var name = 'global';
 * var repository = {name: 'CodeSnippet'};
 * on(div, 'drag', function() {
 *  console.log(this.name);
 * }, repository);
 * // Result when you drag a div: "CodeSnippet"
 */
function on(element, types, handler, context) {
  if (isString(types)) {
    forEach(types.split(/\s+/g), function(type) {
      bindEvent(element, type, handler, context);
    });

    return;
  }

  forEach(types, function(func, type) {
    bindEvent(element, type, func, handler);
  });
}

/**
 * Bind DOM events
 * @param {HTMLElement} element - element to bind events
 * @param {string} type - events name
 * @param {function} handler - handler function or context for handler method
 * @param {object} [context] context - context for handler method.
 * @private
 */
function bindEvent(element, type, handler, context) {
  /**
     * Event handler
     * @param {Event} e - event object
     */
  function eventHandler(e) {
    handler.call(context || element, e || window.event);
  }

  if ('addEventListener' in element) {
    element.addEventListener(type, eventHandler);
  } else if ('attachEvent' in element) {
    element.attachEvent('on' + type, eventHandler);
  }
  memorizeHandler(element, type, handler, eventHandler);
}

/**
 * Memorize DOM event handler for unbinding.
 * @param {HTMLElement} element - element to bind events
 * @param {string} type - events name
 * @param {function} handler - handler function that user passed at on() use
 * @param {function} wrappedHandler - handler function that wrapped by domevent for implementing some features
 * @private
 */
function memorizeHandler(element, type, handler, wrappedHandler) {
  var events = safeEvent(element, type);
  var existInEvents = false;

  forEach(events, function(obj) {
    if (obj.handler === handler) {
      existInEvents = true;

      return false;
    }

    return true;
  });

  if (!existInEvents) {
    events.push({
      handler: handler,
      wrappedHandler: wrappedHandler
    });
  }
}

module.exports = on;


/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Prevent default action
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Prevent default action
 * @param {Event} e - event object
 * @memberof module:domEvent
 */
function preventDefault(e) {
  if (e.preventDefault) {
    e.preventDefault();

    return;
  }

  e.returnValue = false;
}

module.exports = preventDefault;


/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Set className value
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isArray = __webpack_require__(0);
var isUndefined = __webpack_require__(2);

/**
 * Set className value
 * @param {(HTMLElement|SVGElement)} element - target element
 * @param {(string|string[])} cssClass - class names
 * @private
 */
function setClassName(element, cssClass) {
  cssClass = isArray(cssClass) ? cssClass.join(' ') : cssClass;

  cssClass = cssClass.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, '');

  if (isUndefined(element.className.baseVal)) {
    element.className = cssClass;

    return;
  }

  element.className.baseVal = cssClass;
}

module.exports = setClassName;


/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Convert kebab-case
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Convert kebab-case
 * @param {string} key - string to be converted to Kebab-case
 * @private
 */
function convertToKebabCase(key) {
  return key.replace(/([A-Z])/g, function(match) {
    return '-' + match.toLowerCase();
  });
}

module.exports = convertToKebabCase;


/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is an arguments object or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isExisty = __webpack_require__(9);

/**
 * @module type
 */

/**
 * Check whether the given variable is an arguments object or not.
 * If the given variable is an arguments object, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is arguments?
 * @memberof module:type
 */
function isArguments(obj) {
  var result = isExisty(obj) &&
        ((Object.prototype.toString.call(obj) === '[object Arguments]') || !!obj.callee);

  return result;
}

module.exports = isArguments;


/***/ }),
/* 21 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview This module detects the kind of well-known browser and version.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Browser module
 * @module browser
 */

/**
 * This object has an information that indicate the kind of browser. It can detect IE8 ~ IE11, Chrome, Firefox, Safari, and Edge.
 * @memberof module:browser
 * @example
 * var browser = require('tui-code-snippet/browser/browser'); // node, commonjs
 *
 * browser.chrome === true; // chrome
 * browser.firefox === true; // firefox
 * browser.safari === true; // safari
 * browser.msie === true; // IE
 * browser.edge === true; // edge
 * browser.others === true; // other browser
 * browser.version; // browser version
 */
var browser = {
  chrome: false,
  firefox: false,
  safari: false,
  msie: false,
  edge: false,
  others: false,
  version: 0
};

if (typeof window !== 'undefined' && window.navigator) {
  detectBrowser();
}

/**
 * Detect the browser.
 * @private
 */
function detectBrowser() {
  var nav = window.navigator;
  var appName = nav.appName.replace(/\s/g, '_');
  var userAgent = nav.userAgent;

  var rIE = /MSIE\s([0-9]+[.0-9]*)/;
  var rIE11 = /Trident.*rv:11\./;
  var rEdge = /Edge\/(\d+)\./;
  var versionRegex = {
    firefox: /Firefox\/(\d+)\./,
    chrome: /Chrome\/(\d+)\./,
    safari: /Version\/([\d.]+).*Safari\/(\d+)/
  };

  var key, tmp;

  var detector = {
    Microsoft_Internet_Explorer: function() { // eslint-disable-line camelcase
      var detectedVersion = userAgent.match(rIE);

      if (detectedVersion) { // ie8 ~ ie10
        browser.msie = true;
        browser.version = parseFloat(detectedVersion[1]);
      } else { // no version information
        browser.others = true;
      }
    },
    Netscape: function() { // eslint-disable-line complexity
      var detected = false;

      if (rIE11.exec(userAgent)) {
        browser.msie = true;
        browser.version = 11;
        detected = true;
      } else if (rEdge.exec(userAgent)) {
        browser.edge = true;
        browser.version = userAgent.match(rEdge)[1];
        detected = true;
      } else {
        for (key in versionRegex) {
          if (versionRegex.hasOwnProperty(key)) {
            tmp = userAgent.match(versionRegex[key]);
            if (tmp && tmp.length > 1) { // eslint-disable-line max-depth
              browser[key] = detected = true;
              browser.version = parseFloat(tmp[1] || 0);
              break;
            }
          }
        }
      }
      if (!detected) {
        browser.others = true;
      }
    }
  };

  var fn = detector[appName];

  if (fn) {
    detector[appName]();
  }
}

module.exports = browser;


/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Provide a simple inheritance in prototype-oriented.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var createObject = __webpack_require__(23);

/**
 * Provide a simple inheritance in prototype-oriented.
 * Caution :
 *  Don't overwrite the prototype of child constructor.
 *
 * @param {function} subType Child constructor
 * @param {function} superType Parent constructor
 * @memberof module:inheritance
 * @example
 * var inherit = require('tui-code-snippet/inheritance/inherit'); // node, commonjs
 *
 * // Parent constructor
 * function Animal(leg) {
 *     this.leg = leg;
 * }
 * Animal.prototype.growl = function() {
 *     // ...
 * };
 *
 * // Child constructor
 * function Person(name) {
 *     this.name = name;
 * }
 *
 * // Inheritance
 * inherit(Person, Animal);
 *
 * // After this inheritance, please use only the extending of property.
 * // Do not overwrite prototype.
 * Person.prototype.walk = function(direction) {
 *     // ...
 * };
 */
function inherit(subType, superType) {
  var prototype = createObject(superType.prototype);
  prototype.constructor = subType;
  subType.prototype = prototype;
}

module.exports = inherit;


/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Create a new object with the specified prototype object and properties.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * @module inheritance
 */

/**
 * Create a new object with the specified prototype object and properties.
 * @param {Object} obj This object will be a prototype of the newly-created object.
 * @returns {Object}
 * @memberof module:inheritance
 */
function createObject(obj) {
  function F() {} // eslint-disable-line require-jsdoc
  F.prototype = obj;

  return new F();
}

module.exports = createObject;


/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Get event collection for specific HTML element
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var EVENT_KEY = '_feEventKey';

/**
 * Get event collection for specific HTML element
 * @param {HTMLElement} element - HTML element
 * @param {string} type - event type
 * @returns {array}
 * @private
 */
function safeEvent(element, type) {
  var events = element[EVENT_KEY];
  var handlers;

  if (!events) {
    events = element[EVENT_KEY] = {};
  }

  handlers = events[type];
  if (!handlers) {
    handlers = events[type] = [];
  }

  return handlers;
}

module.exports = safeEvent;


/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check element match selector
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var inArray = __webpack_require__(4);
var toArray = __webpack_require__(14);

var elProto = Element.prototype;
var matchSelector = elProto.matches ||
    elProto.webkitMatchesSelector ||
    elProto.mozMatchesSelector ||
    elProto.msMatchesSelector ||
    function(selector) {
      var doc = this.document || this.ownerDocument;

      return inArray(this, toArray(doc.querySelectorAll(selector))) > -1;
    };

/**
 * Check element match selector
 * @param {HTMLElement} element - element to check
 * @param {string} selector - selector to check
 * @returns {boolean} is selector matched to element?
 * @memberof module:domUtil
 */
function matches(element, selector) {
  return matchSelector.call(element, selector);
}

module.exports = matches;


/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Set data attribute to target element
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var convertToKebabCase = __webpack_require__(19);

/**
 * Set data attribute to target element
 * @param {HTMLElement} element - element to set data attribute
 * @param {string} key - key
 * @param {string} value - value
 * @memberof module:domUtil
 */
function setData(element, key, value) {
  if (element.dataset) {
    element.dataset[key] = value;

    return;
  }

  element.setAttribute('data-' + convertToKebabCase(key), value);
}

module.exports = setData;


/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check specific CSS style is available.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check specific CSS style is available.
 * @param {array} props property name to testing
 * @returns {(string|boolean)} return true when property is available
 * @private
 */
function testCSSProp(props) {
  var style = document.documentElement.style;
  var i, len;

  for (i = 0, len = props.length; i < len; i += 1) {
    if (props[i] in style) {
      return props[i];
    }
  }

  return false;
}

module.exports = testCSSProp;


/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Get data value from data-attribute
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var convertToKebabCase = __webpack_require__(19);

/**
 * Get data value from data-attribute
 * @param {HTMLElement} element - target element
 * @param {string} key - key
 * @returns {string} value
 * @memberof module:domUtil
 */
function getData(element, key) {
  if (element.dataset) {
    return element.dataset[key];
  }

  return element.getAttribute('data-' + convertToKebabCase(key));
}

module.exports = getData;


/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Remove data property
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var convertToKebabCase = __webpack_require__(19);

/**
 * Remove data property
 * @param {HTMLElement} element - target element
 * @param {string} key - key
 * @memberof module:domUtil
 */
function removeData(element, key) {
  if (element.dataset) {
    delete element.dataset[key];

    return;
  }

  element.removeAttribute('data-' + convertToKebabCase(key));
}

module.exports = removeData;


/***/ }),
/* 30 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a number or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a number or not.
 * If the given variable is a number, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is number?
 * @memberof module:type
 */
function isNumber(obj) {
  return typeof obj === 'number' || obj instanceof Number;
}

module.exports = isNumber;


/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Retrieve a nested item from the given object/array.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isUndefined = __webpack_require__(2);
var isNull = __webpack_require__(11);

/**
 * Retrieve a nested item from the given object/array.
 * @param {object|Array} obj - Object for retrieving
 * @param {...string|number} paths - Paths of property
 * @returns {*} Value
 * @memberof module:object
 * @example
 * var pick = require('tui-code-snippet/object/pick'); // node, commonjs
 *
 * var obj = {
 *     'key1': 1,
 *     'nested' : {
 *         'key1': 11,
 *         'nested': {
 *             'key1': 21
 *         }
 *     }
 * };
 * pick(obj, 'nested', 'nested', 'key1'); // 21
 * pick(obj, 'nested', 'nested', 'key2'); // undefined
 *
 * var arr = ['a', 'b', 'c'];
 * pick(arr, 1); // 'b'
 */
function pick(obj, paths) { // eslint-disable-line no-unused-vars
  var args = arguments;
  var target = args[0];
  var i = 1;
  var length = args.length;

  for (; i < length; i += 1) {
    if (isUndefined(target) ||
            isNull(target)) {
      return;
    }

    target = target[args[i]];
  }

  return target; // eslint-disable-line consistent-return
}

module.exports = pick;


/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is an instance of Date or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is an instance of Date or not.
 * If the given variables is an instance of Date, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is an instance of Date?
 * @memberof module:type
 */
function isDate(obj) {
  return obj instanceof Date;
}

module.exports = isDate;


/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Request image ping.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEachOwnProperties = __webpack_require__(5);

/**
 * @module request
 */

/**
 * Request image ping.
 * @param {String} url url for ping request
 * @param {Object} trackingInfo infos for make query string
 * @returns {HTMLElement}
 * @memberof module:request
 * @example
 * var imagePing = require('tui-code-snippet/request/imagePing'); // node, commonjs
 *
 * imagePing('https://www.google-analytics.com/collect', {
 *     v: 1,
 *     t: 'event',
 *     tid: 'trackingid',
 *     cid: 'cid',
 *     dp: 'dp',
 *     dh: 'dh'
 * });
 */
function imagePing(url, trackingInfo) {
  var trackingElement = document.createElement('img');
  var queryString = '';
  forEachOwnProperties(trackingInfo, function(value, key) {
    queryString += '&' + key + '=' + value;
  });
  queryString = queryString.substring(1);

  trackingElement.src = url + '?' + queryString;

  trackingElement.style.display = 'none';
  document.body.appendChild(trackingElement);
  document.body.removeChild(trackingElement);

  return trackingElement;
}

module.exports = imagePing;


/***/ }),
/* 34 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Creates a debounced function that delays invoking fn until after delay milliseconds has elapsed since the last time the debouced function was invoked.
 * @author NHN FE Development Lab <dl_javascript.nhn.com>
 */



/**
 * @module tricks
 */

/**
 * Creates a debounced function that delays invoking fn until after delay milliseconds has elapsed
 * since the last time the debouced function was invoked.
 * @param {function} fn The function to debounce.
 * @param {number} [delay=0] The number of milliseconds to delay
 * @returns {function} debounced function.
 * @memberof module:tricks
 * @example
 * var debounce = require('tui-code-snippet/tricks/debounce'); // node, commonjs
 *
 * function someMethodToInvokeDebounced() {}
 *
 * var debounced = debounce(someMethodToInvokeDebounced, 300);
 *
 * // invoke repeatedly
 * debounced();
 * debounced();
 * debounced();
 * debounced();
 * debounced();
 * debounced();    // last invoke of debounced()
 *
 * // invoke someMethodToInvokeDebounced() after 300 milliseconds.
 */
function debounce(fn, delay) {
  var timer, args;

  /* istanbul ignore next */
  delay = delay || 0;

  function debounced() { // eslint-disable-line require-jsdoc
    args = Array.prototype.slice.call(arguments);

    window.clearTimeout(timer);
    timer = window.setTimeout(function() {
      fn.apply(null, args);
    }, delay);
  }

  return debounced;
}

module.exports = debounce;


/***/ }),
/* 35 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is truthy or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isExisty = __webpack_require__(9);

/**
 * Check whether the given variable is truthy or not.
 * If the given variable is not null or not undefined or not false, returns true.
 * (It regards 0 as true)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is truthy?
 * @memberof module:type
 */
function isTruthy(obj) {
  return isExisty(obj) && obj !== false;
}

module.exports = isTruthy;


/***/ }),
/* 36 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * TOAST UI Code Snippet
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



__webpack_require__(37);
__webpack_require__(4);
__webpack_require__(38);
__webpack_require__(39);
__webpack_require__(21);
__webpack_require__(1);
__webpack_require__(3);
__webpack_require__(5);
__webpack_require__(40);
__webpack_require__(14);
__webpack_require__(41);
__webpack_require__(42);
__webpack_require__(43);
__webpack_require__(44);
__webpack_require__(45);
__webpack_require__(15);
__webpack_require__(16);
__webpack_require__(46);
__webpack_require__(17);
__webpack_require__(47);
__webpack_require__(48);
__webpack_require__(49);
__webpack_require__(50);
__webpack_require__(51);
__webpack_require__(52);
__webpack_require__(10);
__webpack_require__(28);
__webpack_require__(53);
__webpack_require__(25);
__webpack_require__(54);
__webpack_require__(29);
__webpack_require__(55);
__webpack_require__(26);
__webpack_require__(56);
__webpack_require__(57);
__webpack_require__(58);
__webpack_require__(59);
__webpack_require__(23);
__webpack_require__(22);
__webpack_require__(6);
__webpack_require__(31);
__webpack_require__(33);
__webpack_require__(60);
__webpack_require__(61);
__webpack_require__(62);
__webpack_require__(34);
__webpack_require__(63);
__webpack_require__(20);
__webpack_require__(0);
__webpack_require__(64);
__webpack_require__(65);
__webpack_require__(66);
__webpack_require__(32);
__webpack_require__(67);
__webpack_require__(13);
__webpack_require__(9);
__webpack_require__(68);
__webpack_require__(12);
__webpack_require__(69);
__webpack_require__(70);
__webpack_require__(71);
__webpack_require__(72);
__webpack_require__(11);
__webpack_require__(30);
__webpack_require__(73);
__webpack_require__(7);
__webpack_require__(8);
__webpack_require__(74);
__webpack_require__(35);
__webpack_require__(2);


/***/ }),
/* 37 */
/***/ (function(__webpack_module__, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _collection_forEachArray__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var _collection_forEachOwnProperties__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(5);
/* harmony import */ var _object_extend__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(6);
/* harmony import */ var _type_isArray__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(0);
/* harmony import */ var _type_isEmpty__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(13);
/* harmony import */ var _type_isFunction__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(12);
/* harmony import */ var _type_isNull__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(11);
/* harmony import */ var _type_isObject__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(7);
/* harmony import */ var _type_isUndefined__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(2);










function encodePairs(key, value) {
  return `${encodeURIComponent(key)}=${encodeURIComponent(
    _type_isNull__WEBPACK_IMPORTED_MODULE_6__(value) || _type_isUndefined__WEBPACK_IMPORTED_MODULE_8__(value) ? '' : value
  )}`;
}

function serializeParams(key, value, serializedList) {
  if (_type_isArray__WEBPACK_IMPORTED_MODULE_3__(value)) {
    _collection_forEachArray__WEBPACK_IMPORTED_MODULE_0__(value, (arrVal, index) => {
      serializeParams(`${key}[${_type_isObject__WEBPACK_IMPORTED_MODULE_7__(arrVal) ? index : ''}]`, arrVal, serializedList);
    });
  } else if (_type_isObject__WEBPACK_IMPORTED_MODULE_7__(value)) {
    _collection_forEachOwnProperties__WEBPACK_IMPORTED_MODULE_1__(value, (objValue, objKey) => {
      serializeParams(`${key}[${objKey}]`, objValue, serializedList);
    });
  } else {
    serializedList.push(encodePairs(key, value));
  }
}

/**
 * Serializer to serialize parameters
 * @callback ajax/serializer
 * @param {*} params - parameter to serialize
 * @returns {string} - serialized strings
 */

/**
 * Serializer
 *
 * 1. Array format
 *
 * The default array format to serialize is 'bracket'.
 * However in case of nested array, only the deepest format follows the 'bracket', the rest follow 'indice' format.
 *
 * - basic
 *   { a: [1, 2, 3] } => a[]=1&a[]=2&a[]=3
 * - nested
 *   { a: [1, 2, [3]] } => a[]=1&a[]=2&a[2][]=3
 *
 * 2. Object format
 *
 * The default object format to serialize is 'bracket' notation and doesn't allow the 'dot' notation.
 *
 * - basic
 *   { a: { b: 1, c: 2 } } => a[b]=1&a[c]=2
 *
 * @param {*} params - parameters to serialize
 * @returns {string}
 * @private
 */
function serialize(params) {
  if (!params || _type_isEmpty__WEBPACK_IMPORTED_MODULE_4__(params)) {
    return '';
  }
  const serializedList = [];
  _collection_forEachOwnProperties__WEBPACK_IMPORTED_MODULE_1__(params, (value, key) => {
    serializeParams(key, value, serializedList);
  });

  return serializedList.join('&');
}

const getDefaultOptions = () => ({
  baseURL: '',
  headers: {
    common: {},
    get: {},
    post: {},
    put: {},
    delete: {},
    patch: {},
    options: {},
    head: {}
  },
  serializer: serialize
});

const HTTP_PROTOCOL_REGEXP = /^(http|https):\/\//i;

/**
 * Combine an absolute URL string (baseURL) and a relative URL string (url).
 * @param {string} baseURL - An absolute URL string
 * @param {string} url - An relative URL string
 * @returns {string}
 * @private
 */
function combineURL(baseURL, url) {
  if (HTTP_PROTOCOL_REGEXP.test(url)) {
    return url;
  }

  if (baseURL.slice(-1) === '/' && url.slice(0, 1) === '/') {
    url = url.slice(1);
  }

  return baseURL + url;
}

/**
 * Get merged options by its priorities.
 * defaults.common < defaults[method] < custom options
 * @param {Object} defaultOptions - The default options
 * @param {Object} customOptions - The custom options
 * @returns {Object}
 * @private
 */
function getComputedOptions(defaultOptions, customOptions) {
  const {
    baseURL,
    headers: defaultHeaders,
    serializer: defaultSerializer,
    beforeRequest: defaultBeforeRequest,
    success: defaultSuccess,
    error: defaultError,
    complete: defaultComplete
  } = defaultOptions;
  const {
    url,
    contentType,
    method,
    params,
    headers,
    serializer,
    beforeRequest,
    success,
    error,
    complete,
    withCredentials,
    mimeType
  } = customOptions;

  const options = {
    url: combineURL(baseURL, url),
    method,
    params,
    headers: _object_extend__WEBPACK_IMPORTED_MODULE_2__(defaultHeaders.common, defaultHeaders[method.toLowerCase()], headers),
    serializer: serializer || defaultSerializer || serialize,
    beforeRequest: [defaultBeforeRequest, beforeRequest],
    success: [defaultSuccess, success],
    error: [defaultError, error],
    complete: [defaultComplete, complete],
    withCredentials,
    mimeType
  };

  options.contentType = contentType || options.headers['Content-Type'];
  delete options.headers['Content-Type'];

  return options;
}

function validateStatus(status) {
  return status >= 200 && status < 300;
}

function hasRequestBody(method) {
  return /^(?:POST|PUT|PATCH)$/.test(method.toUpperCase());
}

function executeCallback(callback, param) {
  if (_type_isArray__WEBPACK_IMPORTED_MODULE_3__(callback)) {
    _collection_forEachArray__WEBPACK_IMPORTED_MODULE_0__(callback, fn => executeCallback(fn, param));
  } else if (_type_isFunction__WEBPACK_IMPORTED_MODULE_5__(callback)) {
    callback(param);
  }
}

function parseHeaders(text) {
  const headers = {};

  _collection_forEachArray__WEBPACK_IMPORTED_MODULE_0__(text.split('\r\n'), header => {
    const [key, value] = header.split(': ');

    if (key !== '' && !_type_isUndefined__WEBPACK_IMPORTED_MODULE_8__(value)) {
      headers[key] = value;
    }
  });

  return headers;
}

function parseJSONData(data) {
  let result = '';
  try {
    result = JSON.parse(data);
  } catch (_) {
    result = data;
  }

  return result;
}

const REQUEST_DONE = 4;

function handleReadyStateChange(xhr, options) {
  const { readyState } = xhr;

  // eslint-disable-next-line eqeqeq
  if (readyState != REQUEST_DONE) {
    return;
  }

  const { status, statusText, responseText } = xhr;
  const { success, resolve, error, reject, complete } = options;

  if (validateStatus(status)) {
    const contentType = xhr.getResponseHeader('Content-Type');
    let data = responseText;

    if (contentType && contentType.indexOf('application/json') > -1) {
      data = parseJSONData(data);
    }

    /**
     * successCallback is executed when the response is received successfully
     * @callback ajax/successCallback
     * @param {Object} response - success response wrapper
     * @param {number} response.status - response status code
     * @param {string} response.statusText - response status text
     * @param {*} response.data - response data. If its Content-Type is 'application/json', the parsed object will be passed.
     * @param {Object.<string,string>} response.headers - response headers
     */
    executeCallback([success, resolve], {
      status,
      statusText,
      data,
      headers: parseHeaders(xhr.getAllResponseHeaders())
    });
  } else {
    /**
     * errorCallback executed when the request failed
     * @callback ajax/errorCallback
     * @param {Object} response - error response wrapper
     * @param {number} response.status - response status code
     * @param {string} response.statusText - response status text
     */
    executeCallback([error, reject], { status, statusText });
  }

  /**
   * completeCallback executed when the request is completed after success or error callbacks are executed
   * @callback ajax/completeCallback
   * @param {Object} response - error response wrapper
   * @param {number} response.status - response status code
   * @param {string} response.statusText - response status text
   */
  executeCallback(complete, { status, statusText });
}

const QS_DELIM_REGEXP = /\?/;

function open(xhr, options) {
  const { url, method, serializer, params } = options;

  let requestUrl = url;

  if (!hasRequestBody(method) && params) {
    // serialize query string
    const qs = (QS_DELIM_REGEXP.test(url) ? '&' : '?') + serializer(params);
    requestUrl = `${url}${qs}`;
  }

  xhr.open(method, requestUrl);
}

function applyConfig(xhr, options) {
  const { method, contentType, mimeType, headers, withCredentials = false } = options;

  // set withCredentials (IE10+)
  if (withCredentials) {
    xhr.withCredentials = withCredentials;
  }

  // override MIME type (IE11+)
  if (mimeType) {
    xhr.overrideMimeType(mimeType);
  }

  _collection_forEachOwnProperties__WEBPACK_IMPORTED_MODULE_1__(headers, (value, header) => {
    if (!_type_isObject__WEBPACK_IMPORTED_MODULE_7__(value)) {
      xhr.setRequestHeader(header, value);
    }
  });

  if (hasRequestBody(method)) {
    xhr.setRequestHeader('Content-Type', `${contentType}; charset=UTF-8`);
  }

  // set 'x-requested-with' header to prevent CSRF in old browser
  xhr.setRequestHeader('x-requested-with', 'XMLHttpRequest');
}

const ENCODED_SPACE_REGEXP = /%20/g;

function send(xhr, options) {
  const {
    method,
    serializer,
    beforeRequest,
    params = {},
    contentType = 'application/x-www-form-urlencoded'
  } = options;

  let body = null;

  if (hasRequestBody(method)) {
    // The space character '%20' is replaced to '+', because application/x-www-form-urlencoded follows rfc-1866
    body =
      contentType.indexOf('application/x-www-form-urlencoded') > -1
        ? serializer(params).replace(ENCODED_SPACE_REGEXP, '+')
        : JSON.stringify(params);
  }

  xhr.onreadystatechange = () => handleReadyStateChange(xhr, options);

  /**
   * beforeRequestCallback is executed before the Ajax request is sent
   * @callback ajax/beforeRequestCallback
   * @param {XMLHttpRequest} xhr - XMLHttpRequest object
   */
  executeCallback(beforeRequest, xhr);
  xhr.send(body);
}

/**
 * @module ajax
 * @description
 * A module for the Ajax request.
 * If the browser supports Promise, return the Promise object. If not, return null.
 * @param {Object} options - Options for the Ajax request
 * @param {string} options.url - URL string
 * @param {('GET'|'POST'|'PUT'|'DELETE'|'PATCH'|'OPTIONS'|'HEAD')} options.method - Method of the Ajax request
 * @param {Object.<string,string>} [options.headers] - Headers for the Ajax request
 * @param {string} [options.contentType] - Content-Type for the Ajax request. It is applied to POST, PUT, and PATCH requests only. Its encoding automatically sets to UTF-8.
 * @param {*} [options.params] - Parameters to send by the Ajax request
 * @param {serializer} [options.serializer] - {@link ajax_serializer Serializer} that determine how to serialize the parameters. Default serializer is {@link https://github.com/nhn/tui.code-snippet/tree/v2.3.0/ajax/index.mjs#L38 serialize()}.
 * @param {beforeRequestCallback} [options.beforeRequest] - {@link ajax_beforeRequestCallback beforeRequest callback} executed before the Ajax request is sent.
 * @param {successCallback} [options.success] - {@link ajax_successCallback success callback} executed when the response is received successfully.
 * @param {errorCallback} [options.error] - {@link ajax_errorCallback error callback} executed when the request failed.
 * @param {completeCallback} [options.complete] - {@link ajax_completeCallback complete callback} executed when the request is completed after success or error callbacks are executed.
 * @param {boolean} [options.withCredentials] - Determine whether cross-site Access-Control requests should be made using credentials or not. This option can be used on IE10+
 * @param {string} [options.mimeType] - Override the MIME type returned by the server. This options can be used on IE11+
 * @returns {?Promise} - If the browser supports Promise, return the Promise object. If not, return null.
 * @example
 * import ajax from 'tui-code-snippet/ajax'; // import ES6 module (written in ES6)
 * // import ajax from 'tui-code-snippet/ajax/index.js'; // import transfiled file (IE8+)
 * // var ajax = require('tui-code-snippet/ajax/index.js'); // commonjs
 *
 * // If the browser supports Promise, return the Promise object
 * ajax({
 *   url: 'https://nhn.github.io/tui-code-snippet/',
 *   method: 'POST',
 *   contentType: 'application/json',
 *   params: {
 *     version: 'v2.3.0',
 *     author: 'NHN. FE Development Lab <dl_javascript@nhn.com>'
 *   },
 *   success: res => console.log(`success: ${res.status} ${res.statusText}`),
 *   error: res => console.log(`error: ${res.status} ${res.statusText}`)
 * }).then(res => console.log(`resolve: ${res.status} ${res.statusText}`))
 *   .catch(res => console.log(`reject: ${res.status} ${res.statusText}`));
 *
 * // If the request succeeds (200, OK)
 * // success: 200 OK
 * // resolve: 200 OK
 *
 * // If the request failed (503, Service Unavailable)
 * // error: 503 Service Unavailable
 * // reject: 503 Service Unavailable
 *
 * // If the browser does not support Promise, return null
 * ajax({
 *   url: 'https://ui.toast.com/fe-guide/',
 *   method: 'GET',
 *   contentType: 'application/json',
 *   params: {
 *     lang: 'en'
 *     title: 'PERFORMANCE',
 *   },
 *   success: res => console.log(`success: ${res.status} ${res.statusText}`),
 *   error: res => console.log(`error: ${res.status} ${res.statusText}`)
 * });
 *
 * // If the request succeeds (200, OK)
 * // success: 200 OK
 *
 * // If the request failed (503, Service Unavailable)
 * // error: 503 Service Unavailable
 */
function ajax(options) {
  const xhr = new XMLHttpRequest();
  const request = opts => _collection_forEachArray__WEBPACK_IMPORTED_MODULE_0__([open, applyConfig, send], fn => fn(xhr, opts));

  options = getComputedOptions(ajax.defaults, options);

  if (typeof Promise !== 'undefined') {
    return new Promise((resolve, reject) => {
      request(_object_extend__WEBPACK_IMPORTED_MODULE_2__(options, { resolve, reject }));
    });
  }

  request(options);

  return null;
}

/**
 * Default configuration for the Ajax request.
 * @property {string} baseURL - baseURL appended with url of ajax options. If url is absolute, baseURL is ignored.
 * ex) baseURL = 'https://nhn.github.io', url = '/tui.code-snippet' => request is sent to 'https://nhn.github.io/tui.code-snippet'
 * ex) baseURL = 'https://nhn.github.io', url = 'https://ui.toast.com' => request is sent to 'https://ui.toast.com'
 * @property {Object} headers - request headers. It extends the header object in the following order: headers.common -> headers\[method\] -> headers in ajax options.
 * @property {Object.<string,string>} headers.common - Common headers regardless of the type of request
 * @property {Object.<string,string>} headers.get - Headers for the GET method
 * @property {Object.<string,string>} headers.post - Headers for the POST method
 * @property {Object.<string,string>} headers.put - Headers for the PUT method
 * @property {Object.<string,string>} headers.delete - Headers for the DELETE method
 * @property {Object.<string,string>} headers.patch - Headers for the PATCH method
 * @property {Object.<string,string>} headers.options - Headers for the OPTIONS method
 * @property {Object.<string,string>} headers.head - Headers for the HEAD method
 * @property {serializer} serializer - {@link ajax_serializer Serializer} that determine how to serialize the parameters. If serializer is specified in both default options and ajax options, use serializer in ajax options.
 * @property {beforeRequestCallback} beforeRequest - {@link ajax_beforeRequestCallback beforeRequest callback} executed before the Ajax request is sent. Callbacks in both default options and ajax options are executed, but default callbacks are called first.
 * @property {successCallback} success - {@link ajax_successCallback success callback} executed when the response is received successfully. Callbacks in both default options and ajax options are executed, but default callbacks are called first.
 * @property {errorCallback} error - {@link ajax_errorCallback error callback} executed when the request failed. Callbacks in both default options and ajax options are executed, but default callbacks are called first.
 * @property {completeCallback} complete - {@link ajax_completeCallback complete callback} executed when the request is completed after success or error callbacks are executed. Callbacks in both default options and ajax options are executed, but default callbacks are called first.
 * @example
 * ajax.defaults.baseURL = 'https://nhn.github.io/tui.code-snippet';
 * ajax.defaults.headers.common = {
 *   'Content-Type': 'application/json'
 * };
 * ajax.defaults.beforeRequest = () => showProgressBar();
 * ajax.defaults.complete = () => hideProgressBar();
 */
ajax.defaults = getDefaultOptions();

/**
 * Reset the default options
 * @private
 */
ajax._reset = function() {
  ajax.defaults = getDefaultOptions();
};

/**
 * Ajax request
 * @private
 */
ajax._request = function(url, method, options = {}) {
  return ajax(_object_extend__WEBPACK_IMPORTED_MODULE_2__(options, { url, method }));
};

/**
 * Send the Ajax request by GET
 * @memberof module:ajax
 * @function get
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.get('https://nhn.github.io/tui.code-snippet/', {
 *   params: {
 *     version: 'v2.3.0'
 *   }
 * });
 */

/**
 * Send the Ajax request by POST
 * @memberof module:ajax
 * @function post
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.post('https://nhn.github.io/tui.code-snippet/', {
 *   contentType: 'application/json',
 *   params: {
 *     version: 'v2.3.0'
 *   }
 * });
 */

/**
 * Send the Ajax request by PUT
 * @memberof module:ajax
 * @function put
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.put('https://nhn.github.io/tui.code-snippet/v2.3.0', {
 *   success: ({status, statusText}) => alert(`success: ${status} ${statusText}`),
 *   error: ({status, statusText}) => alert(`error: ${status} ${statusText}`)
 * });
 */

/**
 * Send the Ajax request by DELETE
 * @memberof module:ajax
 * @function delete
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.delete('https://nhn.github.io/tui.code-snippet/v2.3.0');
 */

/**
 * Send the Ajax request by PATCH
 * @memberof module:ajax
 * @function patch
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.patch('https://nhn.github.io/tui.code-snippet/v2.3.0', {
 *   beforeRequest: () => showProgressBar(),
 *   complete: () => hideProgressBar()
 * });
 */

/**
 * Send the Ajax request by OPTIONS
 * @memberof module:ajax
 * @function options
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.head('https://nhn.github.io/tui.code-snippet/v2.3.0');
 */

/**
 * Send the Ajax request by HEAD
 * @memberof module:ajax
 * @function head
 * @param {string} url - URL string
 * @param {object} options - Options for the Ajax request. Refer to {@link ajax options of ajax()}.
 * @example
 * ajax.options('https://nhn.github.io/tui.code-snippet/v2.3.0');
 */
_collection_forEachArray__WEBPACK_IMPORTED_MODULE_0__(['get', 'post', 'put', 'delete', 'patch', 'options', 'head'], type => {
  ajax[type] = (url, options) => ajax._request(url, type.toUpperCase(), options);
});

/* harmony default export */ __webpack_exports__["default"] = (ajax);


/***/ }),
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Generate an integer Array containing an arithmetic progression.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isUndefined = __webpack_require__(2);

/**
 * Generate an integer Array containing an arithmetic progression.
 * @param {number} start - start index
 * @param {number} stop - stop index
 * @param {number} step - next visit index = current index + step
 * @returns {Array}
 * @memberof module:array
 * @example
 * var range = require('tui-code-snippet/array/range'); // node, commonjs
 *
 * range(5); // [0, 1, 2, 3, 4]
 * range(1, 5); // [1,2,3,4]
 * range(2, 10, 2); // [2,4,6,8]
 * range(10, 2, -2); // [10,8,6,4]
 */
function range(start, stop, step) {
  var arr = [];
  var flag;

  if (isUndefined(stop)) {
    stop = start || 0;
    start = 0;
  }

  step = step || 1;
  flag = step < 0 ? -1 : 1;
  stop *= flag;

  for (; start * flag < stop; start += step) {
    arr.push(start);
  }

  return arr;
}

module.exports = range;


/***/ }),
/* 39 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Zip together multiple lists into a single array.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEach = __webpack_require__(1);

/**
 * Zip together multiple lists into a single array.
 * @param {...Array} ...Arrays - Arrays to be zipped
 * @returns {Array}
 * @memberof module:array
 * @example
 * var zip = require('tui-code-snippet/array/zip'); // node, commonjs
 *
 * var result = zip([1, 2, 3], ['a', 'b','c'], [true, false, true]);
 * console.log(result[0]); // [1, 'a', true]
 * console.log(result[1]); // [2, 'b', false]
 * console.log(result[2]); // [3, 'c', true]
 */
function zip() {
  var arr2d = Array.prototype.slice.call(arguments);
  var result = [];

  forEach(arr2d, function(arr) {
    forEach(arr, function(value, index) {
      if (!result[index]) {
        result[index] = [];
      }
      result[index].push(value);
    });
  });

  return result;
}

module.exports = zip;


/***/ }),
/* 40 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Fetch a property
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEach = __webpack_require__(1);

/**
 * fetching a property
 * @param {Array} arr target collection
 * @param {String|Number} property property name
 * @returns {Array}
 * @memberof module:collection
 * @example
 * var pluck = require('tui-code-snippe/collection/pluck'); // node, commonjs
 *
 * var objArr = [
 *     {'abc': 1, 'def': 2, 'ghi': 3},
 *     {'abc': 4, 'def': 5, 'ghi': 6},
 *     {'abc': 7, 'def': 8, 'ghi': 9}
 * ];
 * var arr2d = [
 *     [1, 2, 3],
 *     [4, 5, 6],
 *     [7, 8, 9]
 * ];
 * pluck(objArr, 'abc'); // [1, 4, 7]
 * pluck(arr2d, 2); // [3, 6, 9]
 */
function pluck(arr, property) {
  var resultArray = [];

  forEach(arr, function(item) {
    resultArray.push(item[property]);
  });

  return resultArray;
}

module.exports = pluck;


/***/ }),
/* 41 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview This module provides some functions for custom events. And it is implemented in the observer design pattern.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var extend = __webpack_require__(6);
var isExisty = __webpack_require__(9);
var isString = __webpack_require__(8);
var isObject = __webpack_require__(7);
var isArray = __webpack_require__(0);
var isFunction = __webpack_require__(12);
var forEach = __webpack_require__(1);

var R_EVENTNAME_SPLIT = /\s+/g;

/**
 * @class
 * @example
 * // node, commonjs
 * var CustomEvents = require('tui-code-snippet/customEvents/customEvents');
 */
function CustomEvents() {
  /**
     * @type {HandlerItem[]}
     */
  this.events = null;

  /**
     * only for checking specific context event was binded
     * @type {object[]}
     */
  this.contexts = null;
}

/**
 * Mixin custom events feature to specific constructor
 * @param {function} func - constructor
 * @example
 * var CustomEvents = require('tui-code-snippet/customEvents/customEvents'); // node, commonjs
 *
 * var model;
 * function Model() {
 *     this.name = '';
 * }
 * CustomEvents.mixin(Model);
 *
 * model = new Model();
 * model.on('change', function() { this.name = 'model'; }, this);
 * model.fire('change');
 * alert(model.name); // 'model';
 */
CustomEvents.mixin = function(func) {
  extend(func.prototype, CustomEvents.prototype);
};

/**
 * Get HandlerItem object
 * @param {function} handler - handler function
 * @param {object} [context] - context for handler
 * @returns {HandlerItem} HandlerItem object
 * @private
 */
CustomEvents.prototype._getHandlerItem = function(handler, context) {
  var item = {handler: handler};

  if (context) {
    item.context = context;
  }

  return item;
};

/**
 * Get event object safely
 * @param {string} [eventName] - create sub event map if not exist.
 * @returns {(object|array)} event object. if you supplied `eventName`
 *  parameter then make new array and return it
 * @private
 */
CustomEvents.prototype._safeEvent = function(eventName) {
  var events = this.events;
  var byName;

  if (!events) {
    events = this.events = {};
  }

  if (eventName) {
    byName = events[eventName];

    if (!byName) {
      byName = [];
      events[eventName] = byName;
    }

    events = byName;
  }

  return events;
};

/**
 * Get context array safely
 * @returns {array} context array
 * @private
 */
CustomEvents.prototype._safeContext = function() {
  var context = this.contexts;

  if (!context) {
    context = this.contexts = [];
  }

  return context;
};

/**
 * Get index of context
 * @param {object} ctx - context that used for bind custom event
 * @returns {number} index of context
 * @private
 */
CustomEvents.prototype._indexOfContext = function(ctx) {
  var context = this._safeContext();
  var index = 0;

  while (context[index]) {
    if (ctx === context[index][0]) {
      return index;
    }

    index += 1;
  }

  return -1;
};

/**
 * Memorize supplied context for recognize supplied object is context or
 *  name: handler pair object when off()
 * @param {object} ctx - context object to memorize
 * @private
 */
CustomEvents.prototype._memorizeContext = function(ctx) {
  var context, index;

  if (!isExisty(ctx)) {
    return;
  }

  context = this._safeContext();
  index = this._indexOfContext(ctx);

  if (index > -1) {
    context[index][1] += 1;
  } else {
    context.push([ctx, 1]);
  }
};

/**
 * Forget supplied context object
 * @param {object} ctx - context object to forget
 * @private
 */
CustomEvents.prototype._forgetContext = function(ctx) {
  var context, contextIndex;

  if (!isExisty(ctx)) {
    return;
  }

  context = this._safeContext();
  contextIndex = this._indexOfContext(ctx);

  if (contextIndex > -1) {
    context[contextIndex][1] -= 1;

    if (context[contextIndex][1] <= 0) {
      context.splice(contextIndex, 1);
    }
  }
};

/**
 * Bind event handler
 * @param {(string|{name:string, handler:function})} eventName - custom
 *  event name or an object {eventName: handler}
 * @param {(function|object)} [handler] - handler function or context
 * @param {object} [context] - context for binding
 * @private
 */
CustomEvents.prototype._bindEvent = function(eventName, handler, context) {
  var events = this._safeEvent(eventName);
  this._memorizeContext(context);
  events.push(this._getHandlerItem(handler, context));
};

/**
 * Bind event handlers
 * @param {(string|{name:string, handler:function})} eventName - custom
 *  event name or an object {eventName: handler}
 * @param {(function|object)} [handler] - handler function or context
 * @param {object} [context] - context for binding
 * //-- #1. Get Module --//
 * var CustomEvents = require('tui-code-snippet/customEvents/customEvents'); // node, commonjs
 *
 * //-- #2. Use method --//
 * // # 2.1 Basic Usage
 * CustomEvents.on('onload', handler);
 *
 * // # 2.2 With context
 * CustomEvents.on('onload', handler, myObj);
 *
 * // # 2.3 Bind by object that name, handler pairs
 * CustomEvents.on({
 *     'play': handler,
 *     'pause': handler2
 * });
 *
 * // # 2.4 Bind by object that name, handler pairs with context object
 * CustomEvents.on({
 *     'play': handler
 * }, myObj);
 */
CustomEvents.prototype.on = function(eventName, handler, context) {
  var self = this;

  if (isString(eventName)) {
    // [syntax 1, 2]
    eventName = eventName.split(R_EVENTNAME_SPLIT);
    forEach(eventName, function(name) {
      self._bindEvent(name, handler, context);
    });
  } else if (isObject(eventName)) {
    // [syntax 3, 4]
    context = handler;
    forEach(eventName, function(func, name) {
      self.on(name, func, context);
    });
  }
};

/**
 * Bind one-shot event handlers
 * @param {(string|{name:string,handler:function})} eventName - custom
 *  event name or an object {eventName: handler}
 * @param {function|object} [handler] - handler function or context
 * @param {object} [context] - context for binding
 */
CustomEvents.prototype.once = function(eventName, handler, context) {
  var self = this;

  if (isObject(eventName)) {
    context = handler;
    forEach(eventName, function(func, name) {
      self.once(name, func, context);
    });

    return;
  }

  function onceHandler() { // eslint-disable-line require-jsdoc
    handler.apply(context, arguments);
    self.off(eventName, onceHandler, context);
  }

  this.on(eventName, onceHandler, context);
};

/**
 * Splice supplied array by callback result
 * @param {array} arr - array to splice
 * @param {function} predicate - function return boolean
 * @private
 */
CustomEvents.prototype._spliceMatches = function(arr, predicate) {
  var i = 0;
  var len;

  if (!isArray(arr)) {
    return;
  }

  for (len = arr.length; i < len; i += 1) {
    if (predicate(arr[i]) === true) {
      arr.splice(i, 1);
      len -= 1;
      i -= 1;
    }
  }
};

/**
 * Get matcher for unbind specific handler events
 * @param {function} handler - handler function
 * @returns {function} handler matcher
 * @private
 */
CustomEvents.prototype._matchHandler = function(handler) {
  var self = this;

  return function(item) {
    var needRemove = handler === item.handler;

    if (needRemove) {
      self._forgetContext(item.context);
    }

    return needRemove;
  };
};

/**
 * Get matcher for unbind specific context events
 * @param {object} context - context
 * @returns {function} object matcher
 * @private
 */
CustomEvents.prototype._matchContext = function(context) {
  var self = this;

  return function(item) {
    var needRemove = context === item.context;

    if (needRemove) {
      self._forgetContext(item.context);
    }

    return needRemove;
  };
};

/**
 * Get matcher for unbind specific hander, context pair events
 * @param {function} handler - handler function
 * @param {object} context - context
 * @returns {function} handler, context matcher
 * @private
 */
CustomEvents.prototype._matchHandlerAndContext = function(handler, context) {
  var self = this;

  return function(item) {
    var matchHandler = (handler === item.handler);
    var matchContext = (context === item.context);
    var needRemove = (matchHandler && matchContext);

    if (needRemove) {
      self._forgetContext(item.context);
    }

    return needRemove;
  };
};

/**
 * Unbind event by event name
 * @param {string} eventName - custom event name to unbind
 * @param {function} [handler] - handler function
 * @private
 */
CustomEvents.prototype._offByEventName = function(eventName, handler) {
  var self = this;
  var andByHandler = isFunction(handler);
  var matchHandler = self._matchHandler(handler);

  eventName = eventName.split(R_EVENTNAME_SPLIT);

  forEach(eventName, function(name) {
    var handlerItems = self._safeEvent(name);

    if (andByHandler) {
      self._spliceMatches(handlerItems, matchHandler);
    } else {
      forEach(handlerItems, function(item) {
        self._forgetContext(item.context);
      });

      self.events[name] = [];
    }
  });
};

/**
 * Unbind event by handler function
 * @param {function} handler - handler function
 * @private
 */
CustomEvents.prototype._offByHandler = function(handler) {
  var self = this;
  var matchHandler = this._matchHandler(handler);

  forEach(this._safeEvent(), function(handlerItems) {
    self._spliceMatches(handlerItems, matchHandler);
  });
};

/**
 * Unbind event by object(name: handler pair object or context object)
 * @param {object} obj - context or {name: handler} pair object
 * @param {function} handler - handler function
 * @private
 */
CustomEvents.prototype._offByObject = function(obj, handler) {
  var self = this;
  var matchFunc;

  if (this._indexOfContext(obj) < 0) {
    forEach(obj, function(func, name) {
      self.off(name, func);
    });
  } else if (isString(handler)) {
    matchFunc = this._matchContext(obj);

    self._spliceMatches(this._safeEvent(handler), matchFunc);
  } else if (isFunction(handler)) {
    matchFunc = this._matchHandlerAndContext(handler, obj);

    forEach(this._safeEvent(), function(handlerItems) {
      self._spliceMatches(handlerItems, matchFunc);
    });
  } else {
    matchFunc = this._matchContext(obj);

    forEach(this._safeEvent(), function(handlerItems) {
      self._spliceMatches(handlerItems, matchFunc);
    });
  }
};

/**
 * Unbind custom events
 * @param {(string|object|function)} eventName - event name or context or
 *  {name: handler} pair object or handler function
 * @param {(function)} handler - handler function
 * @example
 * //-- #1. Get Module --//
 * var CustomEvents = require('tui-code-snippet/customEvents/customEvents'); // node, commonjs
 *
 * //-- #2. Use method --//
 * // # 2.1 off by event name
 * CustomEvents.off('onload');
 *
 * // # 2.2 off by event name and handler
 * CustomEvents.off('play', handler);
 *
 * // # 2.3 off by handler
 * CustomEvents.off(handler);
 *
 * // # 2.4 off by context
 * CustomEvents.off(myObj);
 *
 * // # 2.5 off by context and handler
 * CustomEvents.off(myObj, handler);
 *
 * // # 2.6 off by context and event name
 * CustomEvents.off(myObj, 'onload');
 *
 * // # 2.7 off by an Object.<string, function> that is {eventName: handler}
 * CustomEvents.off({
 *   'play': handler,
 *   'pause': handler2
 * });
 *
 * // # 2.8 off the all events
 * CustomEvents.off();
 */
CustomEvents.prototype.off = function(eventName, handler) {
  if (isString(eventName)) {
    // [syntax 1, 2]
    this._offByEventName(eventName, handler);
  } else if (!arguments.length) {
    // [syntax 8]
    this.events = {};
    this.contexts = [];
  } else if (isFunction(eventName)) {
    // [syntax 3]
    this._offByHandler(eventName);
  } else if (isObject(eventName)) {
    // [syntax 4, 5, 6]
    this._offByObject(eventName, handler);
  }
};

/**
 * Fire custom event
 * @param {string} eventName - name of custom event
 */
CustomEvents.prototype.fire = function(eventName) {  // eslint-disable-line
  this.invoke.apply(this, arguments);
};

/**
 * Fire a event and returns the result of operation 'boolean AND' with all
 *  listener's results.
 *
 * So, It is different from {@link CustomEvents#fire}.
 *
 * In service code, use this as a before event in component level usually
 *  for notifying that the event is cancelable.
 * @param {string} eventName - Custom event name
 * @param {...*} data - Data for event
 * @returns {boolean} The result of operation 'boolean AND'
 * @example
 * var map = new Map();
 * map.on({
 *     'beforeZoom': function() {
 *         // It should cancel the 'zoom' event by some conditions.
 *         if (that.disabled && this.getState()) {
 *             return false;
 *         }
 *         return true;
 *     }
 * });
 *
 * if (this.invoke('beforeZoom')) {    // check the result of 'beforeZoom'
 *     // if true,
 *     // doSomething
 * }
 */
CustomEvents.prototype.invoke = function(eventName) {
  var events, args, index, item;

  if (!this.hasListener(eventName)) {
    return true;
  }

  events = this._safeEvent(eventName);
  args = Array.prototype.slice.call(arguments, 1);
  index = 0;

  while (events[index]) {
    item = events[index];

    if (item.handler.apply(item.context, args) === false) {
      return false;
    }

    index += 1;
  }

  return true;
};

/**
 * Return whether at least one of the handlers is registered in the given
 *  event name.
 * @param {string} eventName - Custom event name
 * @returns {boolean} Is there at least one handler in event name?
 */
CustomEvents.prototype.hasListener = function(eventName) {
  return this.getListenerLength(eventName) > 0;
};

/**
 * Return a count of events registered.
 * @param {string} eventName - Custom event name
 * @returns {number} number of event
 */
CustomEvents.prototype.getListenerLength = function(eventName) {
  var events = this._safeEvent(eventName);

  return events.length;
};

module.exports = CustomEvents;


/***/ }),
/* 42 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview
 * This module provides a function to make a constructor
 * that can inherit from the other constructors like the CLASS easily.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var inherit = __webpack_require__(22);
var extend = __webpack_require__(6);

/**
 * @module defineClass
 */

/**
 * Help a constructor to be defined and to inherit from the other constructors
 * @param {*} [parent] Parent constructor
 * @param {Object} props Members of constructor
 *  @param {Function} props.init Initialization method
 *  @param {Object} [props.static] Static members of constructor
 * @returns {*} Constructor
 * @memberof module:defineClass
 * @example
 * var defineClass = require('tui-code-snippet/defineClass/defineClass'); // node, commonjs
 *
 * //-- #2. Use property --//
 * var Parent = defineClass({
 *     init: function() { // constuructor
 *         this.name = 'made by def';
 *     },
 *     method: function() {
 *         // ...
 *     },
 *     static: {
 *         staticMethod: function() {
 *              // ...
 *         }
 *     }
 * });
 *
 * var Child = defineClass(Parent, {
 *     childMethod: function() {}
 * });
 *
 * Parent.staticMethod();
 *
 * var parentInstance = new Parent();
 * console.log(parentInstance.name); //made by def
 * parentInstance.staticMethod(); // Error
 *
 * var childInstance = new Child();
 * childInstance.method();
 * childInstance.childMethod();
 */
function defineClass(parent, props) {
  var obj;

  if (!props) {
    props = parent;
    parent = null;
  }

  obj = props.init || function() {};

  if (parent) {
    inherit(obj, parent);
  }

  if (props.hasOwnProperty('static')) {
    extend(obj, props['static']);
    delete props['static'];
  }

  extend(obj.prototype, props);

  return obj;
}

module.exports = defineClass;


/***/ }),
/* 43 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Normalize mouse event's button attributes.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var browser = __webpack_require__(21);
var inArray = __webpack_require__(4);

var primaryButton = ['0', '1', '3', '5', '7'];
var secondaryButton = ['2', '6'];
var wheelButton = ['4'];

/**
 * @module domEvent
 */

/**
 * Normalize mouse event's button attributes.
 *
 * Can detect which button is clicked by this method.
 *
 * Meaning of return numbers
 *
 * - 0: primary mouse button
 * - 1: wheel button or center button
 * - 2: secondary mouse button
 * @param {MouseEvent} mouseEvent - The mouse event object want to know.
 * @returns {number} - The value of meaning which button is clicked?
 * @memberof module:domEvent
 */
function getMouseButton(mouseEvent) {
  if (browser.msie && browser.version <= 8) {
    return getMouseButtonIE8AndEarlier(mouseEvent);
  }

  return mouseEvent.button;
}

/**
 * Normalize return value of mouseEvent.button
 * Make same to standard MouseEvent's button value
 * @param {DispCEventObj} mouseEvent - mouse event object
 * @returns {number|null} - id indicating which mouse button is clicked
 * @private
 */
function getMouseButtonIE8AndEarlier(mouseEvent) {
  var button = String(mouseEvent.button);

  if (inArray(button, primaryButton) > -1) {
    return 0;
  }

  if (inArray(button, secondaryButton) > -1) {
    return 2;
  }

  if (inArray(button, wheelButton) > -1) {
    return 1;
  }

  return null;
}

module.exports = getMouseButton;


/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Get mouse position from mouse event
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isArray = __webpack_require__(0);

/**
 * Get mouse position from mouse event
 *
 * If supplied relatveElement parameter then return relative position based on
 *  element
 * @param {(MouseEvent|object|number[])} position - mouse position object
 * @param {HTMLElement} relativeElement HTML element that calculate relative
 *  position
 * @returns {number[]} mouse position
 * @memberof module:domEvent
 */
function getMousePosition(position, relativeElement) {
  var positionArray = isArray(position);
  var clientX = positionArray ? position[0] : position.clientX;
  var clientY = positionArray ? position[1] : position.clientY;
  var rect;

  if (!relativeElement) {
    return [clientX, clientY];
  }

  rect = relativeElement.getBoundingClientRect();

  return [
    clientX - rect.left - relativeElement.clientLeft,
    clientY - rect.top - relativeElement.clientTop
  ];
}

module.exports = getMousePosition;


/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Get a target element from an event object.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Get a target element from an event object.
 * @param {Event} e - event object
 * @returns {HTMLElement} - target element
 * @memberof module:domEvent
 */
function getTarget(e) {
  return e.target || e.srcElement;
}

module.exports = getTarget;


/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Bind DOM event. this event will unbind after invokes.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEachOwnProperties = __webpack_require__(5);
var isObject = __webpack_require__(7);
var on = __webpack_require__(16);
var off = __webpack_require__(15);

/**
 * Bind DOM event. this event will unbind after invokes.
 * @param {HTMLElement} element - HTMLElement to bind events.
 * @param {(string|object)} types - Space splitted events names or
 *  eventName:handler object.
 * @param {(function|object)} handler - handler function or context for handler method.
 * @param {object} [context] - context object for handler method.
 * @memberof module:domEvent
 */
function once(element, types, handler, context) {
  /**
     * Event handler for one time.
     */
  function onceHandler() {
    handler.apply(context || element, arguments);
    off(element, types, onceHandler, context);
  }

  if (isObject(types)) {
    forEachOwnProperties(types, function(fn, type) {
      once(element, type, fn, handler);
    });

    return;
  }

  on(element, types, onceHandler, context);
}

module.exports = once;


/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Stop event propagation.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Stop event propagation.
 * @param {Event} e - event object
 * @memberof module:domEvent
 */
function stopPropagation(e) {
  if (e.stopPropagation) {
    e.stopPropagation();

    return;
  }

  e.cancelBubble = true;
}

module.exports = stopPropagation;


/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Add css class to element
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEach = __webpack_require__(1);
var inArray = __webpack_require__(4);
var getClass = __webpack_require__(10);
var setClassName = __webpack_require__(18);

/**
 * domUtil module
 * @module domUtil
 */

/**
 * Add css class to element
 * @param {(HTMLElement|SVGElement)} element - target element
 * @param {...string} cssClass - css classes to add
 * @memberof module:domUtil
 */
function addClass(element) {
  var cssClass = Array.prototype.slice.call(arguments, 1);
  var classList = element.classList;
  var newClass = [];
  var origin;

  if (classList) {
    forEach(cssClass, function(name) {
      element.classList.add(name);
    });

    return;
  }

  origin = getClass(element);

  if (origin) {
    cssClass = [].concat(origin.split(/\s+/), cssClass);
  }

  forEach(cssClass, function(cls) {
    if (inArray(cls, newClass) < 0) {
      newClass.push(cls);
    }
  });

  setClassName(element, newClass);
}

module.exports = addClass;


/***/ }),
/* 49 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Find parent element recursively
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var matches = __webpack_require__(25);

/**
 * Find parent element recursively
 * @param {HTMLElement} element - base element to start find
 * @param {string} selector - selector string for find
 * @returns {HTMLElement} - element finded or null
 * @memberof module:domUtil
 */
function closest(element, selector) {
  var parent = element.parentNode;

  if (matches(element, selector)) {
    return element;
  }

  while (parent && parent !== document) {
    if (matches(parent, selector)) {
      return parent;
    }

    parent = parent.parentNode;
  }

  return null;
}

module.exports = closest;


/***/ }),
/* 50 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Setting element style
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isString = __webpack_require__(8);
var forEach = __webpack_require__(1);

/**
 * Setting element style
 * @param {(HTMLElement|SVGElement)} element - element to setting style
 * @param {(string|object)} key - style prop name or {prop: value} pair object
 * @param {string} [value] - style value
 * @memberof module:domUtil
 */
function css(element, key, value) {
  var style = element.style;

  if (isString(key)) {
    style[key] = value;

    return;
  }

  forEach(key, function(v, k) {
    style[k] = v;
  });
}

module.exports = css;


/***/ }),
/* 51 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Disable browser's text selection behaviors.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var on = __webpack_require__(16);
var preventDefault = __webpack_require__(17);
var setData = __webpack_require__(26);
var testCSSProp = __webpack_require__(27);

var SUPPORT_SELECTSTART = 'onselectstart' in document;
var KEY_PREVIOUS_USER_SELECT = 'prevUserSelect';
var userSelectProperty = testCSSProp([
  'userSelect',
  'WebkitUserSelect',
  'OUserSelect',
  'MozUserSelect',
  'msUserSelect'
]);

/**
 * Disable browser's text selection behaviors.
 * @param {HTMLElement} [el] - target element. if not supplied, use `document`
 * @memberof module:domUtil
 */
function disableTextSelection(el) {
  if (!el) {
    el = document;
  }

  if (SUPPORT_SELECTSTART) {
    on(el, 'selectstart', preventDefault);
  } else {
    el = (el === document) ? document.documentElement : el;
    setData(el, KEY_PREVIOUS_USER_SELECT, el.style[userSelectProperty]);
    el.style[userSelectProperty] = 'none';
  }
}

module.exports = disableTextSelection;


/***/ }),
/* 52 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Transform the Array-like object to Array.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var off = __webpack_require__(15);
var preventDefault = __webpack_require__(17);
var getData = __webpack_require__(28);
var removeData = __webpack_require__(29);
var testCSSProp = __webpack_require__(27);

var SUPPORT_SELECTSTART = 'onselectstart' in document;
var KEY_PREVIOUS_USER_SELECT = 'prevUserSelect';
var userSelectProperty = testCSSProp([
  'userSelect',
  'WebkitUserSelect',
  'OUserSelect',
  'MozUserSelect',
  'msUserSelect'
]);

/**
 * Enable browser's text selection behaviors.
 * @param {HTMLElement} [el] - target element. if not supplied, use `document`
 * @memberof module:domUtil
 */
function enableTextSelection(el) {
  if (!el) {
    el = document;
  }

  if (SUPPORT_SELECTSTART) {
    off(el, 'selectstart', preventDefault);
  } else {
    el = (el === document) ? document.documentElement : el;
    el.style[userSelectProperty] = getData(el, KEY_PREVIOUS_USER_SELECT) || 'auto';
    removeData(el, KEY_PREVIOUS_USER_SELECT);
  }
}

module.exports = enableTextSelection;


/***/ }),
/* 53 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check element has specific css class
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var inArray = __webpack_require__(4);
var getClass = __webpack_require__(10);

/**
 * Check element has specific css class
 * @param {(HTMLElement|SVGElement)} element - target element
 * @param {string} cssClass - css class
 * @returns {boolean}
 * @memberof module:domUtil
 */
function hasClass(element, cssClass) {
  var origin;

  if (element.classList) {
    return element.classList.contains(cssClass);
  }

  origin = getClass(element).split(/\s+/);

  return inArray(cssClass, origin) > -1;
}

module.exports = hasClass;


/***/ }),
/* 54 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Remove css class from element
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEachArray = __webpack_require__(3);
var inArray = __webpack_require__(4);
var getClass = __webpack_require__(10);
var setClassName = __webpack_require__(18);

/**
 * Remove css class from element
 * @param {(HTMLElement|SVGElement)} element - target element
 * @param {...string} cssClass - css classes to remove
 * @memberof module:domUtil
 */
function removeClass(element) {
  var cssClass = Array.prototype.slice.call(arguments, 1);
  var classList = element.classList;
  var origin, newClass;

  if (classList) {
    forEachArray(cssClass, function(name) {
      classList.remove(name);
    });

    return;
  }

  origin = getClass(element).split(/\s+/);
  newClass = [];
  forEachArray(origin, function(name) {
    if (inArray(name, cssClass) < 0) {
      newClass.push(name);
    }
  });

  setClassName(element, newClass);
}

module.exports = removeClass;


/***/ }),
/* 55 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Remove element from parent node.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Remove element from parent node.
 * @param {HTMLElement} element - element to remove.
 * @memberof module:domUtil
 */
function removeElement(element) {
  if (element && element.parentNode) {
    element.parentNode.removeChild(element);
  }
}

module.exports = removeElement;


/***/ }),
/* 56 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Convert text by binding expressions with context.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var inArray = __webpack_require__(4);
var forEach = __webpack_require__(1);
var isArray = __webpack_require__(0);
var isString = __webpack_require__(8);
var extend = __webpack_require__(6);

// IE8 does not support capture groups.
var EXPRESSION_REGEXP = /{{\s?|\s?}}/g;
var BRACKET_NOTATION_REGEXP = /^[a-zA-Z0-9_@]+\[[a-zA-Z0-9_@"']+\]$/;
var BRACKET_REGEXP = /\[\s?|\s?\]/;
var DOT_NOTATION_REGEXP = /^[a-zA-Z_]+\.[a-zA-Z_]+$/;
var DOT_REGEXP = /\./;
var STRING_NOTATION_REGEXP = /^["']\w+["']$/;
var STRING_REGEXP = /"|'/g;
var NUMBER_REGEXP = /^-?\d+\.?\d*$/;

var EXPRESSION_INTERVAL = 2;

var BLOCK_HELPERS = {
  'if': handleIf,
  'each': handleEach,
  'with': handleWith
};

var isValidSplit = 'a'.split(/a/).length === 3;

/**
 * Split by RegExp. (Polyfill for IE8)
 * @param {string} text - text to be splitted\
 * @param {RegExp} regexp - regular expression
 * @returns {Array.<string>}
 */
var splitByRegExp = (function() {
  if (isValidSplit) {
    return function(text, regexp) {
      return text.split(regexp);
    };
  }

  return function(text, regexp) {
    var result = [];
    var prevIndex = 0;
    var match, index;

    if (!regexp.global) {
      regexp = new RegExp(regexp, 'g');
    }

    match = regexp.exec(text);
    while (match !== null) {
      index = match.index;
      result.push(text.slice(prevIndex, index));

      prevIndex = index + match[0].length;
      match = regexp.exec(text);
    }
    result.push(text.slice(prevIndex));

    return result;
  };
})();

/**
 * Find value in the context by an expression.
 * @param {string} exp - an expression
 * @param {object} context - context
 * @returns {*}
 * @private
 */
// eslint-disable-next-line complexity
function getValueFromContext(exp, context) {
  var splitedExps;
  var value = context[exp];

  if (exp === 'true') {
    value = true;
  } else if (exp === 'false') {
    value = false;
  } else if (STRING_NOTATION_REGEXP.test(exp)) {
    value = exp.replace(STRING_REGEXP, '');
  } else if (BRACKET_NOTATION_REGEXP.test(exp)) {
    splitedExps = exp.split(BRACKET_REGEXP);
    value = getValueFromContext(splitedExps[0], context)[getValueFromContext(splitedExps[1], context)];
  } else if (DOT_NOTATION_REGEXP.test(exp)) {
    splitedExps = exp.split(DOT_REGEXP);
    value = getValueFromContext(splitedExps[0], context)[splitedExps[1]];
  } else if (NUMBER_REGEXP.test(exp)) {
    value = parseFloat(exp);
  }

  return value;
}

/**
 * Extract elseif and else expressions.
 * @param {Array.<string>} ifExps - args of if expression
 * @param {Array.<string>} sourcesInsideBlock - sources inside if block
 * @returns {object} - exps: expressions of if, elseif, and else / sourcesInsideIf: sources inside if, elseif, and else block.
 * @private
 */
function extractElseif(ifExps, sourcesInsideBlock) {
  var exps = [ifExps];
  var sourcesInsideIf = [];
  var otherIfCount = 0;
  var start = 0;

  // eslint-disable-next-line complexity
  forEach(sourcesInsideBlock, function(source, index) {
    if (source.indexOf('if') === 0) {
      otherIfCount += 1;
    } else if (source === '/if') {
      otherIfCount -= 1;
    } else if (!otherIfCount && (source.indexOf('elseif') === 0 || source === 'else')) {
      exps.push(source === 'else' ? ['true'] : source.split(' ').slice(1));
      sourcesInsideIf.push(sourcesInsideBlock.slice(start, index));
      start = index + 1;
    }
  });

  sourcesInsideIf.push(sourcesInsideBlock.slice(start));

  return {
    exps: exps,
    sourcesInsideIf: sourcesInsideIf
  };
}

/**
 * Helper function for "if". 
 * @param {Array.<string>} exps - array of expressions split by spaces
 * @param {Array.<string>} sourcesInsideBlock - array of sources inside the if block
 * @param {object} context - context
 * @returns {string}
 * @private
 */
function handleIf(exps, sourcesInsideBlock, context) {
  var analyzed = extractElseif(exps, sourcesInsideBlock);
  var result = false;
  var compiledSource = '';

  forEach(analyzed.exps, function(exp, index) {
    result = handleExpression(exp, context);
    if (result) {
      compiledSource = compile(analyzed.sourcesInsideIf[index], context);
    }

    return !result;
  });

  return compiledSource;
}

/**
 * Helper function for "each".
 * @param {Array.<string>} exps - array of expressions split by spaces
 * @param {Array.<string>} sourcesInsideBlock - array of sources inside the each block
 * @param {object} context - context
 * @returns {string}
 * @private
 */
function handleEach(exps, sourcesInsideBlock, context) {
  var collection = handleExpression(exps, context);
  var additionalKey = isArray(collection) ? '@index' : '@key';
  var additionalContext = {};
  var result = '';

  forEach(collection, function(item, key) {
    additionalContext[additionalKey] = key;
    additionalContext['@this'] = item;
    extend(context, additionalContext);

    result += compile(sourcesInsideBlock.slice(), context);
  });

  return result;
}

/**
 * Helper function for "with ... as"
 * @param {Array.<string>} exps - array of expressions split by spaces
 * @param {Array.<string>} sourcesInsideBlock - array of sources inside the with block
 * @param {object} context - context
 * @returns {string}
 * @private
 */
function handleWith(exps, sourcesInsideBlock, context) {
  var asIndex = inArray('as', exps);
  var alias = exps[asIndex + 1];
  var result = handleExpression(exps.slice(0, asIndex), context);

  var additionalContext = {};
  additionalContext[alias] = result;

  return compile(sourcesInsideBlock, extend(context, additionalContext)) || '';
}

/**
 * Extract sources inside block in place.
 * @param {Array.<string>} sources - array of sources
 * @param {number} start - index of start block
 * @param {number} end - index of end block
 * @returns {Array.<string>}
 * @private
 */
function extractSourcesInsideBlock(sources, start, end) {
  var sourcesInsideBlock = sources.splice(start + 1, end - start);
  sourcesInsideBlock.pop();

  return sourcesInsideBlock;
}

/**
 * Handle block helper function
 * @param {string} helperKeyword - helper keyword (ex. if, each, with)
 * @param {Array.<string>} sourcesToEnd - array of sources after the starting block
 * @param {object} context - context
 * @returns {Array.<string>}
 * @private
 */
function handleBlockHelper(helperKeyword, sourcesToEnd, context) {
  var executeBlockHelper = BLOCK_HELPERS[helperKeyword];
  var helperCount = 1;
  var startBlockIndex = 0;
  var endBlockIndex;
  var index = startBlockIndex + EXPRESSION_INTERVAL;
  var expression = sourcesToEnd[index];

  while (helperCount && isString(expression)) {
    if (expression.indexOf(helperKeyword) === 0) {
      helperCount += 1;
    } else if (expression.indexOf('/' + helperKeyword) === 0) {
      helperCount -= 1;
      endBlockIndex = index;
    }

    index += EXPRESSION_INTERVAL;
    expression = sourcesToEnd[index];
  }

  if (helperCount) {
    throw Error(helperKeyword + ' needs {{/' + helperKeyword + '}} expression.');
  }

  sourcesToEnd[startBlockIndex] = executeBlockHelper(
    sourcesToEnd[startBlockIndex].split(' ').slice(1),
    extractSourcesInsideBlock(sourcesToEnd, startBlockIndex, endBlockIndex),
    context
  );

  return sourcesToEnd;
}

/**
 * Helper function for "custom helper".
 * If helper is not a function, return helper itself.
 * @param {Array.<string>} exps - array of expressions split by spaces (first element: helper)
 * @param {object} context - context
 * @returns {string}
 * @private
 */
function handleExpression(exps, context) {
  var result = getValueFromContext(exps[0], context);

  if (result instanceof Function) {
    return executeFunction(result, exps.slice(1), context);
  }

  return result;
}

/**
 * Execute a helper function.
 * @param {Function} helper - helper function
 * @param {Array.<string>} argExps - expressions of arguments
 * @param {object} context - context
 * @returns {string} - result of executing the function with arguments
 * @private
 */
function executeFunction(helper, argExps, context) {
  var args = [];
  forEach(argExps, function(exp) {
    args.push(getValueFromContext(exp, context));
  });

  return helper.apply(null, args);
}

/**
 * Get a result of compiling an expression with the context.
 * @param {Array.<string>} sources - array of sources split by regexp of expression.
 * @param {object} context - context
 * @returns {Array.<string>} - array of sources that bind with its context
 * @private
 */
function compile(sources, context) {
  var index = 1;
  var expression = sources[index];
  var exps, firstExp, result;

  while (isString(expression)) {
    exps = expression.split(' ');
    firstExp = exps[0];

    if (BLOCK_HELPERS[firstExp]) {
      result = handleBlockHelper(firstExp, sources.splice(index, sources.length - index), context);
      sources = sources.concat(result);
    } else {
      sources[index] = handleExpression(exps, context);
    }

    index += EXPRESSION_INTERVAL;
    expression = sources[index];
  }

  return sources.join('');
}

/**
 * Convert text by binding expressions with context.
 * <br>
 * If expression exists in the context, it will be replaced.
 * ex) '{{title}}' with context {title: 'Hello!'} is converted to 'Hello!'.
 * An array or object can be accessed using bracket and dot notation.
 * ex) '{{odds\[2\]}}' with context {odds: \[1, 3, 5\]} is converted to '5'.
 * ex) '{{evens\[first\]}}' with context {evens: \[2, 4\], first: 0} is converted to '2'.
 * ex) '{{project\["name"\]}}' and '{{project.name}}' with context {project: {name: 'CodeSnippet'}} is converted to 'CodeSnippet'.
 * <br>
 * If replaced expression is a function, next expressions will be arguments of the function.
 * ex) '{{add 1 2}}' with context {add: function(a, b) {return a + b;}} is converted to '3'.
 * <br>
 * It has 3 predefined block helpers '{{helper ...}} ... {{/helper}}': 'if', 'each', 'with ... as ...'.
 * 1) 'if' evaluates conditional statements. It can use with 'elseif' and 'else'.
 * 2) 'each' iterates an array or object. It provides '@index'(array), '@key'(object), and '@this'(current element).
 * 3) 'with ... as ...' provides an alias.
 * @param {string} text - text with expressions
 * @param {object} context - context
 * @returns {string} - text that bind with its context
 * @memberof module:domUtil
 * @example
 * var template = require('tui-code-snippet/domUtil/template');
 * 
 * var source = 
 *     '<h1>'
 *   +   '{{if isValidNumber title}}'
 *   +     '{{title}}th'
 *   +   '{{elseif isValidDate title}}'
 *   +     'Date: {{title}}'
 *   +   '{{/if}}'
 *   + '</h1>'
 *   + '{{each list}}'
 *   +   '{{with addOne @index as idx}}'
 *   +     '<p>{{idx}}: {{@this}}</p>'
 *   +   '{{/with}}'
 *   + '{{/each}}';
 * 
 * var context = {
 *   isValidDate: function(text) {
 *     return /^\d{4}-(0|1)\d-(0|1|2|3)\d$/.test(text);
 *   },
 *   isValidNumber: function(text) {
 *     return /^\d+$/.test(text);
 *   }
 *   title: '2019-11-25',
 *   list: ['Clean the room', 'Wash the dishes'],
 *   addOne: function(num) {
 *     return num + 1;
 *   }
 * };
 * 
 * var result = template(source, context);
 * console.log(result); // <h1>Date: 2019-11-25</h1><p>1: Clean the room</p><p>2: Wash the dishes</p>
 */
function template(text, context) {
  return compile(splitByRegExp(text, EXPRESSION_REGEXP), context);
}

module.exports = template;


/***/ }),
/* 57 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Toggle css class
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var forEach = __webpack_require__(1);
var inArray = __webpack_require__(4);
var getClass = __webpack_require__(10);
var setClassName = __webpack_require__(18);

/**
 * Toggle css class
 * @param {(HTMLElement|SVGElement)} element - target element
 * @param {...string} cssClass - css classes to toggle
 * @memberof module:domUtil
 */
function toggleClass(element) {
  var cssClass = Array.prototype.slice.call(arguments, 1);
  var newClass;

  if (element.classList) {
    forEach(cssClass, function(name) {
      element.classList.toggle(name);
    });

    return;
  }

  newClass = getClass(element).split(/\s+/);

  forEach(cssClass, function(name) {
    var idx = inArray(name, newClass);

    if (idx > -1) {
      newClass.splice(idx, 1);
    } else {
      newClass.push(name);
    }
  });

  setClassName(element, newClass);
}

module.exports = toggleClass;


/***/ }),
/* 58 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview This module provides a Enum Constructor.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 * @example
 * // node, commonjs
 * var Enum = require('tui-code-snippet/enum/enum');
 */



var isNumber = __webpack_require__(30);
var isArray = __webpack_require__(0);
var toArray = __webpack_require__(14);
var forEach = __webpack_require__(1);

/**
 * Check whether the defineProperty() method is supported.
 * @type {boolean}
 * @ignore
 */
var isSupportDefinedProperty = (function() {
  try {
    Object.defineProperty({}, 'x', {});

    return true;
  } catch (e) {
    return false;
  }
})();

/**
 * A unique value of a constant.
 * @type {number}
 * @ignore
 */
var enumValue = 0;

/**
 * Make a constant-list that has unique values.
 * In modern browsers (except IE8 and lower),
 *  a value defined once can not be changed.
 *
 * @param {...string|string[]} itemList Constant-list (An array of string is available)
 * @class
 *
 * @example
 * var Enum = require('tui-code-snippet/enum/enum'); // node, commonjs
 *
 * var MYENUM = new Enum('TYPE1', 'TYPE2');
 * var MYENUM2 = new Enum(['TYPE1', 'TYPE2']);
 *
 * //usage
 * if (value === MYENUM.TYPE1) {
 *      ....
 * }
 *
 * //add (If a duplicate name is inputted, will be disregarded.)
 * MYENUM.set('TYPE3', 'TYPE4');
 *
 * //get name of a constant by a value
 * MYENUM.getName(MYENUM.TYPE1); // 'TYPE1'
 *
 * // In modern browsers (except IE8 and lower), a value can not be changed in constants.
 * var originalValue = MYENUM.TYPE1;
 * MYENUM.TYPE1 = 1234; // maybe TypeError
 * MYENUM.TYPE1 === originalValue; // true
 **/
function Enum(itemList) {
  if (itemList) {
    this.set.apply(this, arguments);
  }
}

/**
 * Define a constants-list
 * @param {...string|string[]} itemList Constant-list (An array of string is available)
 */
Enum.prototype.set = function(itemList) {
  var self = this;

  if (!isArray(itemList)) {
    itemList = toArray(arguments);
  }

  forEach(itemList, function itemListIteratee(item) {
    self._addItem(item);
  });
};

/**
 * Return a key of the constant.
 * @param {number} value A value of the constant.
 * @returns {string|undefined} Key of the constant.
 */
Enum.prototype.getName = function(value) {
  var self = this;
  var foundedKey;

  forEach(this, function(itemValue, key) { // eslint-disable-line consistent-return
    if (self._isEnumItem(key) && value === itemValue) {
      foundedKey = key;

      return false;
    }
  });

  return foundedKey;
};

/**
 * Create a constant.
 * @private
 * @param {string} name Constant name. (It will be a key of a constant)
 */
Enum.prototype._addItem = function(name) {
  var value;

  if (!this.hasOwnProperty(name)) {
    value = this._makeEnumValue();

    if (isSupportDefinedProperty) {
      Object.defineProperty(this, name, {
        enumerable: true,
        configurable: false,
        writable: false,
        value: value
      });
    } else {
      this[name] = value;
    }
  }
};

/**
 * Return a unique value for assigning to a constant.
 * @private
 * @returns {number} A unique value
 */
Enum.prototype._makeEnumValue = function() {
  var value;

  value = enumValue;
  enumValue += 1;

  return value;
};

/**
 * Return whether a constant from the given key is in instance or not.
 * @param {string} key - A constant key
 * @returns {boolean} Result
 * @private
 */
Enum.prototype._isEnumItem = function(key) {
  return isNumber(this[key]);
};

module.exports = Enum;


/***/ }),
/* 59 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview This module has a function for date format.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var pick = __webpack_require__(31);
var isDate = __webpack_require__(32);

var tokens = /[\\]*YYYY|[\\]*YY|[\\]*MMMM|[\\]*MMM|[\\]*MM|[\\]*M|[\\]*DD|[\\]*D|[\\]*HH|[\\]*H|[\\]*A/gi;
var MONTH_STR = [
  'Invalid month', 'January', 'February', 'March', 'April', 'May',
  'June', 'July', 'August', 'September', 'October', 'November', 'December'
];
var MONTH_DAYS = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
var replaceMap = {
  M: function(date) {
    return Number(date.month);
  },
  MM: function(date) {
    var month = date.month;

    return (Number(month) < 10) ? '0' + month : month;
  },
  MMM: function(date) {
    return MONTH_STR[Number(date.month)].substr(0, 3);
  },
  MMMM: function(date) {
    return MONTH_STR[Number(date.month)];
  },
  D: function(date) {
    return Number(date.date);
  },
  d: function(date) {
    return replaceMap.D(date); // eslint-disable-line new-cap
  },
  DD: function(date) {
    var dayInMonth = date.date;

    return (Number(dayInMonth) < 10) ? '0' + dayInMonth : dayInMonth;
  },
  dd: function(date) {
    return replaceMap.DD(date); // eslint-disable-line new-cap
  },
  YY: function(date) {
    return Number(date.year) % 100;
  },
  yy: function(date) {
    return replaceMap.YY(date); // eslint-disable-line new-cap
  },
  YYYY: function(date) {
    var prefix = '20',
      year = date.year;
    if (year > 69 && year < 100) {
      prefix = '19';
    }

    return (Number(year) < 100) ? prefix + String(year) : year;
  },
  yyyy: function(date) {
    return replaceMap.YYYY(date); // eslint-disable-line new-cap
  },
  A: function(date) {
    return date.meridiem;
  },
  a: function(date) {
    return date.meridiem;
  },
  hh: function(date) {
    var hour = date.hour;

    return (Number(hour) < 10) ? '0' + hour : hour;
  },
  HH: function(date) {
    return replaceMap.hh(date);
  },
  h: function(date) {
    return String(Number(date.hour));
  },
  H: function(date) {
    return replaceMap.h(date);
  },
  m: function(date) {
    return String(Number(date.minute));
  },
  mm: function(date) {
    var minute = date.minute;

    return (Number(minute) < 10) ? '0' + minute : minute;
  }
};

/**
 * Check whether the given variables are valid date or not.
 * @param {number} year - Year
 * @param {number} month - Month
 * @param {number} date - Day in month.
 * @returns {boolean} Is valid?
 * @private
 */
function isValidDate(year, month, date) { // eslint-disable-line complexity
  var isValidYear, isValidMonth, isValid, lastDayInMonth;

  year = Number(year);
  month = Number(month);
  date = Number(date);

  isValidYear = (year > -1 && year < 100) || ((year > 1969) && (year < 2070));
  isValidMonth = (month > 0) && (month < 13);

  if (!isValidYear || !isValidMonth) {
    return false;
  }

  lastDayInMonth = MONTH_DAYS[month];
  if (month === 2 && year % 4 === 0) {
    if (year % 100 !== 0 || year % 400 === 0) {
      lastDayInMonth = 29;
    }
  }

  isValid = (date > 0) && (date <= lastDayInMonth);

  return isValid;
}

/**
 * @module formatDate
 */

/**
 * Return a string that transformed from the given form and date.
 * @param {string} form - Date form
 * @param {Date|Object} date - Date object
 * @param {{meridiemSet: {AM: string, PM: string}}} option - Option
 * @returns {boolean|string} A transformed string or false.
 * @memberof module:formatDate
 * @example
 *  // key             | Shorthand
 *  // --------------- |-----------------------
 *  // years           | YY / YYYY / yy / yyyy
 *  // months(n)       | M / MM
 *  // months(str)     | MMM / MMMM
 *  // days            | D / DD / d / dd
 *  // hours           | H / HH / h / hh
 *  // minutes         | m / mm
 *  // meridiem(AM,PM) | A / a
 *
 * var formatDate = require('tui-code-snippet/formatDate/formatDate'); // node, commonjs
 *
 * var dateStr1 = formatDate('yyyy-MM-dd', {
 *     year: 2014,
 *     month: 12,
 *     date: 12
 * });
 * alert(dateStr1); // '2014-12-12'
 *
 * var dateStr2 = formatDate('MMM DD YYYY HH:mm', {
 *     year: 1999,
 *     month: 9,
 *     date: 9,
 *     hour: 0,
 *     minute: 2
 * });
 * alert(dateStr2); // 'Sep 09 1999 00:02'
 *
 * var dt = new Date(2010, 2, 13),
 *     dateStr3 = formatDate('yyyy년 M월 dd일', dt);
 * alert(dateStr3); // '2010년 3월 13일'
 *
 * var option4 = {
 *     meridiemSet: {
 *         AM: '오전',
 *         PM: '오후'
 *     }
 * };
 * var date4 = {year: 1999, month: 9, date: 9, hour: 13, minute: 2};
 * var dateStr4 = formatDate('yyyy-MM-dd A hh:mm', date4, option4));
 * alert(dateStr4); // '1999-09-09 오후 01:02'
 */
function formatDate(form, date, option) { // eslint-disable-line complexity
  var am = pick(option, 'meridiemSet', 'AM') || 'AM';
  var pm = pick(option, 'meridiemSet', 'PM') || 'PM';
  var meridiem, nDate, resultStr;

  if (isDate(date)) {
    nDate = {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      date: date.getDate(),
      hour: date.getHours(),
      minute: date.getMinutes()
    };
  } else {
    nDate = {
      year: date.year,
      month: date.month,
      date: date.date,
      hour: date.hour,
      minute: date.minute
    };
  }

  if (!isValidDate(nDate.year, nDate.month, nDate.date)) {
    return false;
  }

  nDate.meridiem = '';
  if (/([^\\]|^)[aA]\b/.test(form)) {
    meridiem = (nDate.hour > 11) ? pm : am;
    if (nDate.hour > 12) { // See the clock system: https://en.wikipedia.org/wiki/12-hour_clock
      nDate.hour %= 12;
    }
    if (nDate.hour === 0) {
      nDate.hour = 12;
    }
    nDate.meridiem = meridiem;
  }

  resultStr = form.replace(tokens, function(key) {
    if (key.indexOf('\\') > -1) { // escape character
      return key.replace(/\\/, '');
    }

    return replaceMap[key](nDate) || '';
  });

  return resultStr;
}

module.exports = formatDate;


/***/ }),
/* 60 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Send hostname on DOMContentLoaded.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isUndefined = __webpack_require__(2);
var imagePing = __webpack_require__(33);

var ms7days = 7 * 24 * 60 * 60 * 1000;

/**
 * Check if the date has passed 7 days
 * @param {number} date - milliseconds
 * @returns {boolean}
 * @private
 */
function isExpired(date) {
  var now = new Date().getTime();

  return now - date > ms7days;
}

/**
 * Send hostname on DOMContentLoaded.
 * To prevent hostname set tui.usageStatistics to false.
 * @param {string} appName - application name
 * @param {string} trackingId - GA tracking ID
 * @ignore
 */
function sendHostname(appName, trackingId) {
  var url = 'https://www.google-analytics.com/collect';
  var hostname = location.hostname;
  var hitType = 'event';
  var eventCategory = 'use';
  var applicationKeyForStorage = 'TOAST UI ' + appName + ' for ' + hostname + ': Statistics';
  var date = window.localStorage.getItem(applicationKeyForStorage);

  // skip if the flag is defined and is set to false explicitly
  if (!isUndefined(window.tui) && window.tui.usageStatistics === false) {
    return;
  }

  // skip if not pass seven days old
  if (date && !isExpired(date)) {
    return;
  }

  window.localStorage.setItem(applicationKeyForStorage, new Date().getTime());

  setTimeout(function() {
    if (document.readyState === 'interactive' || document.readyState === 'complete') {
      imagePing(url, {
        v: 1,
        t: hitType,
        tid: trackingId,
        cid: hostname,
        dp: hostname,
        dh: appName,
        el: appName,
        ec: eventCategory
      });
    }
  }, 1000);
}

module.exports = sendHostname;


/***/ }),
/* 61 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Transform the given HTML Entity string into plain string.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * @module string
 */

/**
 * Transform the given HTML Entity string into plain string.
 * @param {String} htmlEntity - HTML Entity type string
 * @returns {String} Plain string
 * @memberof module:string
 * @example
 * var decodeHTMLEntity = require('tui-code-snippet/string/decodeHTMLEntity'); // node, commonjs
 *
 * var htmlEntityString = "A &#39;quote&#39; is &lt;b&gt;bold&lt;/b&gt;"
 * var result = decodeHTMLEntity(htmlEntityString); //"A 'quote' is <b>bold</b>"
 */
function decodeHTMLEntity(htmlEntity) {
  var entities = {
    '&quot;': '"',
    '&amp;': '&',
    '&lt;': '<',
    '&gt;': '>',
    '&#39;': '\'',
    '&nbsp;': ' '
  };

  return htmlEntity.replace(/&amp;|&lt;|&gt;|&quot;|&#39;|&nbsp;/g, function(m0) {
    return entities[m0] ? entities[m0] : m0;
  });
}

module.exports = decodeHTMLEntity;


/***/ }),
/* 62 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Transform the given string into HTML Entity string.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Transform the given string into HTML Entity string.
 * @param {String} html - String for encoding
 * @returns {String} HTML Entity
 * @memberof module:string
 * @example
 * var encodeHTMLEntity = require('tui-code-snippet/string/encodeHTMLEntity'); // node, commonjs
 *
 * var htmlEntityString = "<script> alert('test');</script><a href='test'>";
 * var result = encodeHTMLEntity(htmlEntityString);
 */
function encodeHTMLEntity(html) {
  var entities = {
    '"': 'quot',
    '&': 'amp',
    '<': 'lt',
    '>': 'gt',
    '\'': '#39'
  };

  return html.replace(/[<>&"']/g, function(m0) {
    return entities[m0] ? '&' + entities[m0] + ';' : m0;
  });
}

module.exports = encodeHTMLEntity;


/***/ }),
/* 63 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Creates a throttled function that only invokes fn at most once per every interval milliseconds.
 * @author NHN FE Development Lab <dl_javascript.nhn.com>
 */



var debounce = __webpack_require__(34);

/**
 * Creates a throttled function that only invokes fn at most once per every interval milliseconds.
 * You can use this throttle short time repeatedly invoking functions. (e.g MouseMove, Resize ...)
 * if you need reuse throttled method. you must remove slugs (e.g. flag variable) related with throttling.
 * @param {function} fn function to throttle
 * @param {number} [interval=0] the number of milliseconds to throttle invocations to.
 * @returns {function} throttled function
 * @memberof module:tricks
 * @example
 * var throttle = require('tui-code-snippet/tricks/throttle'); // node, commonjs
 *
 * function someMethodToInvokeThrottled() {}
 *
 * var throttled = throttle(someMethodToInvokeThrottled, 300);
 *
 * // invoke repeatedly
 * throttled();    // invoke (leading)
 * throttled();
 * throttled();    // invoke (near 300 milliseconds)
 * throttled();
 * throttled();
 * throttled();    // invoke (near 600 milliseconds)
 * // ...
 * // invoke (trailing)
 *
 * // if you need reuse throttled method. then invoke reset()
 * throttled.reset();
 */
function throttle(fn, interval) {
  var base;
  var isLeading = true;
  var tick = function(_args) {
    fn.apply(null, _args);
    base = null;
  };
  var debounced, stamp, args;

  /* istanbul ignore next */
  interval = interval || 0;

  debounced = debounce(tick, interval);

  function throttled() { // eslint-disable-line require-jsdoc
    args = Array.prototype.slice.call(arguments);

    if (isLeading) {
      tick(args);
      isLeading = false;

      return;
    }

    stamp = Number(new Date());

    base = base || stamp;

    // pass array directly because `debounce()`, `tick()` are already use
    // `apply()` method to invoke developer's `fn` handler.
    //
    // also, this `debounced` line invoked every time for implements
    // `trailing` features.
    debounced(args);

    if ((stamp - base) >= interval) {
      tick(args);
    }
  }

  function reset() { // eslint-disable-line require-jsdoc
    isLeading = true;
    base = null;
  }

  throttled.reset = reset;

  return throttled;
}

module.exports = throttle;


/***/ }),
/* 64 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is an instance of Array or not. (for multiple frame environments)
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is an instance of Array or not.
 * If the given variable is an instance of Array, return true.
 * (It is used for multiple frame environments)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is an instance of array?
 * @memberof module:type
 */
function isArraySafe(obj) {
  return Object.prototype.toString.call(obj) === '[object Array]';
}

module.exports = isArraySafe;


/***/ }),
/* 65 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a string or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a boolean or not.
 *  If the given variable is a boolean, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is boolean?
 * @memberof module:type
 */
function isBoolean(obj) {
  return typeof obj === 'boolean' || obj instanceof Boolean;
}

module.exports = isBoolean;


/***/ }),
/* 66 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a boolean or not. (for multiple frame environments)
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a boolean or not.
 * If the given variable is a boolean, return true.
 * (It is used for multiple frame environments)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is a boolean?
 * @memberof module:type
 */
function isBooleanSafe(obj) {
  return Object.prototype.toString.call(obj) === '[object Boolean]';
}

module.exports = isBooleanSafe;


/***/ }),
/* 67 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is an instance of Date or not. (for multiple frame environments)
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is an instance of Date or not.
 * If the given variables is an instance of Date, return true.
 * (It is used for multiple frame environments)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is an instance of Date?
 * @memberof module:type
 */
function isDateSafe(obj) {
  return Object.prototype.toString.call(obj) === '[object Date]';
}

module.exports = isDateSafe;


/***/ }),
/* 68 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is falsy or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isTruthy = __webpack_require__(35);

/**
 * Check whether the given variable is falsy or not.
 * If the given variable is null or undefined or false, returns true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is falsy?
 * @memberof module:type
 */
function isFalsy(obj) {
  return !isTruthy(obj);
}

module.exports = isFalsy;


/***/ }),
/* 69 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a function or not. (for multiple frame environments)
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a function or not.
 * If the given variable is a function, return true.
 * (It is used for multiple frame environments)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is a function?
 * @memberof module:type
 */
function isFunctionSafe(obj) {
  return Object.prototype.toString.call(obj) === '[object Function]';
}

module.exports = isFunctionSafe;


/***/ }),
/* 70 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a instance of HTMLNode or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a instance of HTMLNode or not.
 * If the given variables is a instance of HTMLNode, return true.
 * @param {*} html - Target for checking
 * @returns {boolean} Is HTMLNode ?
 * @memberof module:type
 */
function isHTMLNode(html) {
  if (typeof HTMLElement === 'object') {
    return (html && (html instanceof HTMLElement || !!html.nodeType));
  }

  return !!(html && html.nodeType);
}

module.exports = isHTMLNode;


/***/ }),
/* 71 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a HTML tag or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a HTML tag or not.
 * If the given variables is a HTML tag, return true.
 * @param {*} html - Target for checking
 * @returns {boolean} Is HTML tag?
 * @memberof module:type
 */
function isHTMLTag(html) {
  if (typeof HTMLElement === 'object') {
    return (html && (html instanceof HTMLElement));
  }

  return !!(html && html.nodeType && html.nodeType === 1);
}

module.exports = isHTMLTag;


/***/ }),
/* 72 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is not empty(not null, not undefined, or not empty array, not empty object) or not.
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



var isEmpty = __webpack_require__(13);

/**
 * Check whether the given variable is not empty
 * (not null, not undefined, or not empty array, not empty object) or not.
 * If the given variables is not empty, return true.
 * @param {*} obj - Target for checking
 * @returns {boolean} Is not empty?
 * @memberof module:type
 */
function isNotEmpty(obj) {
  return !isEmpty(obj);
}

module.exports = isNotEmpty;


/***/ }),
/* 73 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a number or not. (for multiple frame environments)
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a number or not.
 * If the given variable is a number, return true.
 * (It is used for multiple frame environments)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is a number?
 * @memberof module:type
 */
function isNumberSafe(obj) {
  return Object.prototype.toString.call(obj) === '[object Number]';
}

module.exports = isNumberSafe;


/***/ }),
/* 74 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * @fileoverview Check whether the given variable is a string or not. (for multiple frame environments)
 * @author NHN FE Development Lab <dl_javascript@nhn.com>
 */



/**
 * Check whether the given variable is a string or not.
 * If the given variable is a string, return true.
 * (It is used for multiple frame environments)
 * @param {*} obj - Target for checking
 * @returns {boolean} Is a string?
 * @memberof module:type
 */
function isStringSafe(obj) {
  return Object.prototype.toString.call(obj) === '[object String]';
}

module.exports = isStringSafe;


/***/ })
/******/ ]);
});