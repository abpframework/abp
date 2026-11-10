/*!
 * jQuery Migrate - v4.0.2 - 2026-01-21T12:38Z
 * Copyright OpenJS Foundation and other contributors
 * Released under the MIT license
 * https://jquery.com/license/
 */
( function( factory ) {
	"use strict";

	if ( typeof define === "function" && define.amd ) {

		// AMD. Register as an anonymous module.
		define( [ "jquery" ], function( jQuery ) {
			return factory( jQuery, window );
		} );
	} else if ( typeof module === "object" && module.exports ) {

		// Node/CommonJS
		// eslint-disable-next-line no-undef
		module.exports = factory( require( "jquery" ), window );
	} else {

		// Browser globals
		factory( jQuery, window );
	}
} )( function( jQuery, window ) {
"use strict";

jQuery.migrateVersion = "4.0.2";

// Returns 0 if v1 == v2, -1 if v1 < v2, 1 if v1 > v2
function compareVersions( v1, v2 ) {
	var i,
		rVersionParts = /^(\d+)\.(\d+)\.(\d+)/,
		v1p = rVersionParts.exec( v1 ) || [ ],
		v2p = rVersionParts.exec( v2 ) || [ ];

	for ( i = 1; i <= 3; i++ ) {
		if ( +v1p[ i ] > +v2p[ i ] ) {
			return 1;
		}
		if ( +v1p[ i ] < +v2p[ i ] ) {
			return -1;
		}
	}
	return 0;
}

function jQueryVersionSince( version ) {
	return compareVersions( jQuery.fn.jquery, version ) >= 0;
}

// A map from disabled patch codes to `true`. This should really
// be a `Set` but those are unsupported in IE.
var disabledPatches = Object.create( null );

// Don't apply patches for specified codes. Helpful for code bases
// where some Migrate warnings have been addressed, and it's desirable
// to avoid needless patches or false positives.
jQuery.migrateDisablePatches = function() {
	var i;
	for ( i = 0; i < arguments.length; i++ ) {
		disabledPatches[ arguments[ i ] ] = true;
	}
};

// Allow enabling patches disabled via `jQuery.migrateDisablePatches`.
// Helpful if you want to disable a patch only for some code that won't
// be updated soon to be able to focus on other warnings - and enable it
// immediately after such a call:
// ```js
// jQuery.migrateDisablePatches( "workaroundA" );
// elem.pluginViolatingWarningA( "pluginMethod" );
// jQuery.migrateEnablePatches( "workaroundA" );
// ```
jQuery.migrateEnablePatches = function() {
	var i;
	for ( i = 0; i < arguments.length; i++ ) {
		delete disabledPatches[ arguments[ i ] ];
	}
};

jQuery.migrateIsPatchEnabled = function( patchCode ) {
	return !disabledPatches[ patchCode ];
};

( function() {

// Need jQuery 4.x and no older Migrate loaded
if ( !jQuery || !jQueryVersionSince( "4.0.0" ) ||
		jQueryVersionSince( "5.0.0" ) ) {
	window.console.log( "JQMIGRATE: jQuery 4.x REQUIRED" );
}
if ( jQuery.migrateMessages ) {
	window.console.log( "JQMIGRATE: Migrate plugin loaded multiple times" );
}

// Show a message on the console so devs know we're active
window.console.log( "JQMIGRATE: Migrate is installed" +
	( jQuery.migrateMute ? "" : " with logging active" ) +
	", version " + jQuery.migrateVersion );

} )();

var messagesLogged = Object.create( null );

// List of warnings already given; public read only
jQuery.migrateMessages = [];

// By default, each warning is only reported once.
if ( jQuery.migrateDeduplicateMessages === undefined ) {
	jQuery.migrateDeduplicateMessages = true;
}

// Set to `false` to disable traces that appear with warnings
if ( jQuery.migrateTrace === undefined ) {
	jQuery.migrateTrace = true;
}

// Forget any warnings we've already given; public
jQuery.migrateReset = function() {
	messagesLogged = Object.create( null );
	jQuery.migrateMessages.length = 0;
};

function migrateMessageInternal( code, msg, consoleMethod ) {
	var console = window.console;

	if ( jQuery.migrateIsPatchEnabled( code ) &&
		( !jQuery.migrateDeduplicateMessages || !messagesLogged[ msg ] ) ) {
		messagesLogged[ msg ] = true;
		jQuery.migrateMessages.push( consoleMethod.toUpperCase() + ": " +
			msg + " [" + code + "]" );

		if ( console[ consoleMethod ] && !jQuery.migrateMute ) {
			console[ consoleMethod ]( "JQMIGRATE: " + msg );

			if ( jQuery.migrateTrace ) {

				// Label the trace so that filtering messages in DevTools
				// doesn't hide traces. Note that IE ignores the label.
				console.trace( "JQMIGRATE: " + msg );
			}
		}
	}
}

function migrateWarn( code, msg ) {
	migrateMessageInternal( code, msg, "warn" );
}

function migrateInfo( code, msg ) {
	migrateMessageInternal( code, msg, "info" );
}

function migratePatchPropInternal(
	obj, prop, value, code, msg, migrateMessageFn
) {
	var orig = obj[ prop ];
	Object.defineProperty( obj, prop, {
		configurable: true,
		enumerable: true,

		get: function() {
			if ( jQuery.migrateIsPatchEnabled( code ) ) {

				// If `msg` not provided, do not message; more sophisticated
				// messaging logic is most likely embedded in `value`.
				if ( msg ) {
					migrateMessageFn( code, msg );
				}

				return value;
			}

			return orig;
		},

		set: function( newValue ) {
			if ( jQuery.migrateIsPatchEnabled( code ) ) {

				// See the comment in the getter.
				if ( msg ) {
					migrateMessageFn( code, msg );
				}
			}

			// If a new value was set, apply it regardless if
			// the patch is later disabled or not.
			orig = value = newValue;
		}
	} );
}

function migrateWarnProp( obj, prop, value, code, msg ) {
	migratePatchPropInternal( obj, prop, value, code, msg, migrateWarn );
}

function migrateInfoProp( obj, prop, value, code, msg ) {
	if ( !msg ) {
		throw new Error( "No warning message provided" );
	}
	migratePatchPropInternal( obj, prop, value, code, msg, migrateInfo );
}

function migratePatchProp( obj, prop, value, code ) {
	migratePatchPropInternal( obj, prop, value, code );
}

// The value of the "Func" APIs is that for method we want to allow
// checking for the method existence without triggering a warning.
// For other deprecated properties, we do need to warn on access.
function migratePatchFuncInternal(
	obj, prop, newFunc, code, msg, migrateMessageFn
) {

	function wrappedNewFunc() {

		// If `msg` not provided, do not warn; more sophisticated warnings
		// logic is most likely embedded in `newFunc`, in that case here
		// we just care about the logic choosing the proper implementation
		// based on whether the patch is disabled or not.
		if ( msg ) {
			migrateMessageFn( code, msg );
		}

		return newFunc.apply( this, arguments );
	}

	migratePatchPropInternal( obj, prop, wrappedNewFunc, code );
}

function migratePatchAndWarnFunc( obj, prop, newFunc, code, msg ) {
	if ( !msg ) {
		throw new Error( "No warning message provided" );
	}
	return migratePatchFuncInternal( obj, prop, newFunc, code, msg, migrateWarn );
}

function migratePatchAndInfoFunc( obj, prop, newFunc, code, msg ) {
	if ( !msg ) {
		throw new Error( "No info message provided" );
	}
	return migratePatchFuncInternal( obj, prop, newFunc, code, msg, migrateInfo );
}

function migratePatchFunc( obj, prop, newFunc, code ) {
	return migratePatchFuncInternal( obj, prop, newFunc, code );
}

if ( window.document.compatMode === "BackCompat" ) {

	// jQuery has never supported or tested Quirks Mode
	migrateWarn( "quirks", "jQuery is not compatible with Quirks Mode" );
}

var arr = [],
	push = arr.push,
	sort = arr.sort,
	splice = arr.splice,
	class2type = {},

	// Matches dashed string for camelizing
	rmsPrefix = /^-ms-/,
	rdashAlpha = /-([a-z])/g,

	// Require that the "whitespace run" starts from a non-whitespace
	// to avoid O(N^2) behavior when the engine would try matching "\s+$" at each space position.
	rtrim = /^[\s\uFEFF\xA0]+|([^\s\uFEFF\xA0])[\s\uFEFF\xA0]+$/g;

migratePatchAndWarnFunc( jQuery, "parseJSON", function() {
	return JSON.parse.apply( null, arguments );
}, "parseJSON",
"jQuery.parseJSON is removed; use JSON.parse" );

migratePatchAndInfoFunc( jQuery, "holdReady", jQuery.holdReady,
	"holdReady", "jQuery.holdReady() is deprecated" );

migratePatchAndWarnFunc( jQuery, "unique", jQuery.uniqueSort,
	"unique", "jQuery.unique() is removed; use jQuery.uniqueSort()" );

migratePatchAndWarnFunc( jQuery, "trim", function( text ) {
	return text == null ?
		"" :
		( text + "" ).replace( rtrim, "$1" );
}, "trim",
"jQuery.trim() is removed; use String.prototype.trim" );

migratePatchAndWarnFunc( jQuery, "nodeName", function( elem, name ) {
	return elem.nodeName && elem.nodeName.toLowerCase() === name.toLowerCase();
}, "nodeName",
"jQuery.nodeName() is removed" );

migratePatchAndWarnFunc( jQuery, "isArray", Array.isArray, "isArray",
	"jQuery.isArray() is removed; use Array.isArray()"
);

migratePatchAndWarnFunc( jQuery, "isNumeric",
	function( obj ) {

		// As of jQuery 3.0, isNumeric is limited to
		// strings and numbers (primitives or objects)
		// that can be coerced to finite numbers (gh-2662)
		var type = typeof obj;
		return ( type === "number" || type === "string" ) &&

			// parseFloat NaNs numeric-cast false positives ("")
			// ...but misinterprets leading-number strings, e.g. hex literals ("0x...")
			// subtraction forces infinities to NaN
			!isNaN( obj - parseFloat( obj ) );
	}, "isNumeric",
	"jQuery.isNumeric() is removed"
);

// Populate the class2type map
jQuery.each( "Boolean Number String Function Array Date RegExp Object Error Symbol".
	split( " " ),
function( _, name ) {
	class2type[ "[object " + name + "]" ] = name.toLowerCase();
} );

migratePatchAndWarnFunc( jQuery, "type", function( obj ) {
	if ( obj == null ) {
		return obj + "";
	}

	return typeof obj === "object" ?
		class2type[ Object.prototype.toString.call( obj ) ] || "object" :
		typeof obj;
}, "type",
"jQuery.type() is removed" );

migratePatchAndWarnFunc( jQuery, "isFunction", function( obj ) {
	return typeof obj === "function";
}, "isFunction",
"jQuery.isFunction() is removed" );

migratePatchAndWarnFunc( jQuery, "isWindow",
	function( obj ) {
		return obj != null && obj === obj.window;
	}, "isWindow",
	"jQuery.isWindow() is removed"
);

migratePatchAndWarnFunc( jQuery, "now", Date.now, "now",
	"jQuery.now() is removed; use Date.now()"
);

// Used by camelCase as callback to replace()
function fcamelCase( _all, letter ) {
	return letter.toUpperCase();
}

migratePatchAndWarnFunc( jQuery, "camelCase",
	function( string ) {

		// Convert dashed to camelCase; used by the css and data modules
		// Support: IE <=9 - 11, Edge 12 - 15
		// Microsoft forgot to hump their vendor prefix (trac-9572)
		return string.replace( rmsPrefix, "ms-" ).replace( rdashAlpha, fcamelCase );
	}, "camelCase",
	"jQuery.camelCase() is removed"
);

// Bind a function to a context, optionally partially applying any
// arguments.
// jQuery.proxy is deprecated to promote standards (specifically Function#bind)
// However, it is not slated for removal any time soon
migratePatchAndInfoFunc( jQuery, "proxy", jQuery.proxy,
	"proxy", "jQuery.proxy() is deprecated" );

migratePatchAndWarnFunc( jQuery.fn, "push", push, "push",
	"jQuery.fn.push() is removed; use .add() or convert to an array" );
migratePatchAndWarnFunc( jQuery.fn, "sort", sort, "sort",
	"jQuery.fn.sort() is removed; convert to an array before sorting" );
migratePatchAndWarnFunc( jQuery.fn, "splice", splice, "splice",
	"jQuery.fn.splice() is removed; use .slice() or .not() with .eq()" );

// Now jQuery.expr.pseudos is the standard incantation
migrateInfoProp( jQuery.expr, "filters", jQuery.expr.pseudos, "expr-pre-pseudos",
	"jQuery.expr.filters is deprecated; use jQuery.expr.pseudos" );
migrateInfoProp( jQuery.expr, ":", jQuery.expr.pseudos, "expr-pre-pseudos",
	"jQuery.expr[':'] is deprecated; use jQuery.expr.pseudos" );

function markFunction( fn ) {
	fn[ jQuery.expando ] = true;
	return fn;
}

migratePatchFunc( jQuery.expr.filter, "PSEUDO", function( pseudo, argument ) {

	// pseudo-class names are case-insensitive
	// https://www.w3.org/TR/selectors/#pseudo-classes
	// Prioritize by case sensitivity in case custom pseudos are added with uppercase letters
	// Remember that setFilters inherits from pseudos
	var args,
		fn = jQuery.expr.pseudos[ pseudo ] ||
			jQuery.expr.setFilters[ pseudo.toLowerCase() ] ||
			jQuery.error(
				"Syntax error, unrecognized expression: unsupported pseudo: " +
					pseudo );

	// The user may use createPseudo to indicate that
	// arguments are needed to create the filter function
	// just as jQuery does
	if ( fn[ jQuery.expando ] ) {
		return fn( argument );
	}

	// But maintain support for old signatures
	if ( fn.length > 1 ) {
		migrateInfo( "legacy-custom-pseudos",
			"Pseudos with multiple arguments are deprecated; " +
				"use jQuery.expr.createPseudo()" );
		args = [ pseudo, pseudo, "", argument ];
		return jQuery.expr.setFilters.hasOwnProperty( pseudo.toLowerCase() ) ?
			markFunction( function( seed, matches ) {
				var idx,
					matched = fn( seed, argument ),
					i = matched.length;
				while ( i-- ) {
					idx = Array.prototype.indexOf.call( seed, matched[ i ] );
					seed[ idx ] = !( matches[ idx ] = matched[ i ] );
				}
			} ) :
			function( elem ) {
				return fn( elem, 0, args );
			};
	}

	return fn;
}, "legacy-custom-pseudos" );

if ( typeof Proxy !== "undefined" ) {
	jQuery.each( [ "pseudos", "setFilters" ], function( _, api ) {
		jQuery.expr[ api ] = new Proxy( jQuery.expr[ api ], {
			set: function( _target, _prop, fn ) {
				if ( typeof fn === "function" && !fn[ jQuery.expando ] && fn.length > 1 ) {
					migrateInfo( "legacy-custom-pseudos",
						"Pseudos with multiple arguments are deprecated; " +
							"use jQuery.expr.createPseudo()" );
				}
				return Reflect.set.apply( this, arguments );
			}
		} );
	} );
}

// Support jQuery slim which excludes the ajax module
if ( jQuery.ajax ) {

var oldCallbacks = [],
	guid = "migrate-" + Date.now(),
	origJsonpCallback = jQuery.ajaxSettings.jsonpCallback,
	rjsonp = /(=)\?(?=&|$)|\?\?/,
	rquery = /\?/;

jQuery.ajaxSetup( {
	jsonpCallback: function() {

		// Source is virtually the same as in Core, but we need to duplicate
		// to maintain a proper `oldCallbacks` reference.
		if ( jQuery.migrateIsPatchEnabled( "jsonp-promotion" ) ) {
			var callback = oldCallbacks.pop() || ( jQuery.expando + "_" + ( guid++ ) );
			this[ callback ] = true;
			return callback;
		} else {
			return origJsonpCallback.apply( this, arguments );
		}
	}
} );

// Register this prefilter before the jQuery one. Otherwise, a promoted
// request is transformed into one with the script dataType, and we can't
// catch it anymore.
//
// Code mostly from:
// https://github.com/jquery/jquery/blob/fa0058af426c4e482059214c29c29f004254d9a1/src/ajax/jsonp.js#L20-L97
jQuery.ajaxPrefilter( "+json", function( s, originalSettings, jqXHR ) {

	if ( !jQuery.migrateIsPatchEnabled( "jsonp-promotion" ) ) {
		return;
	}

	var callbackName, overwritten, responseContainer,
		jsonProp = s.jsonp !== false && ( rjsonp.test( s.url ) ?
			"url" :
			typeof s.data === "string" &&
				( s.contentType || "" )
					.indexOf( "application/x-www-form-urlencoded" ) === 0 &&
				rjsonp.test( s.data ) && "data"
		);

	// Handle iff the expected data type is "jsonp" or we have a parameter to set
	if ( jsonProp || s.dataTypes[ 0 ] === "jsonp" ) {
		migrateWarn( "jsonp-promotion", "JSON-to-JSONP auto-promotion is removed" );

		// Get callback name, remembering preexisting value associated with it
		callbackName = s.jsonpCallback = typeof s.jsonpCallback === "function" ?
			s.jsonpCallback() :
			s.jsonpCallback;

		// Insert callback into url or form data
		if ( jsonProp ) {
			s[ jsonProp ] = s[ jsonProp ].replace( rjsonp, "$1" + callbackName );
		} else if ( s.jsonp !== false ) {
			s.url += ( rquery.test( s.url ) ? "&" : "?" ) + s.jsonp + "=" + callbackName;
		}

		// Use data converter to retrieve json after script execution
		s.converters[ "script json" ] = function() {
			if ( !responseContainer ) {
				jQuery.error( callbackName + " was not called" );
			}
			return responseContainer[ 0 ];
		};

		// Force json dataType
		s.dataTypes[ 0 ] = "json";

		// Install callback
		overwritten = window[ callbackName ];
		window[ callbackName ] = function() {
			responseContainer = arguments;
		};

		// Clean-up function (fires after converters)
		jqXHR.always( function() {

			// If previous value didn't exist - remove it
			if ( overwritten === undefined ) {
				jQuery( window ).removeProp( callbackName );

				// Otherwise restore preexisting value
			} else {
				window[ callbackName ] = overwritten;
			}

			// Save back as free
			if ( s[ callbackName ] ) {

				// Make sure that re-using the options doesn't screw things around
				s.jsonpCallback = originalSettings.jsonpCallback;

				// Save the callback name for future use
				oldCallbacks.push( callbackName );
			}

			// Call if it was a function and we have a response
			if ( responseContainer && typeof overwritten === "function" ) {
				overwritten( responseContainer[ 0 ] );
			}

			responseContainer = overwritten = undefined;
		} );

		// Delegate to script
		return "script";
	}
} );

// Don't trigger the above logic by default as the JSON-to-JSONP auto-promotion
// behavior is gone in jQuery 4.0 and as it has security implications, we don't
// want to restore the legacy behavior by default.
jQuery.migrateDisablePatches( "jsonp-promotion" );

}

var oldJQueryAttr = jQuery.attr,
	oldToggleClass = jQuery.fn.toggleClass,
	booleans = "checked|selected|async|autofocus|autoplay|controls|defer|" +
		"disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped",
	rbooleans = new RegExp( "^(?:" + booleans + ")$", "i" ),

	// Some formerly boolean attributes gained new values with special meaning.
	// Skip the old boolean attr logic for those values.
	extraBoolAttrValues = {
		hidden: [ "until-found" ]
	};

// HTML boolean attributes have special behavior:
// we consider the lowercase name to be the only valid value, so
// getting (if the attribute is present) normalizes to that, as does
// setting to any non-`false` value (and setting to `false` removes the attribute).
// See https://html.spec.whatwg.org/multipage/common-microsyntaxes.html#boolean-attributes
jQuery.each( booleans.split( "|" ), function( _i, name ) {
	var origAttrHooks = jQuery.attrHooks[ name ] || {};
	jQuery.attrHooks[ name ] = {
		get: origAttrHooks.get || function( elem ) {
			var attrValue;

			if ( jQuery.migrateIsPatchEnabled( "boolean-attributes" ) ) {
				attrValue = elem.getAttribute( name );

				if ( attrValue !== name && attrValue != null &&
					( extraBoolAttrValues[ name ] || [] )
						.indexOf( String( attrValue ).toLowerCase() ) === -1
				) {
					migrateWarn( "boolean-attributes",
						"Boolean attribute '" + name +
							"' value is different from its lowercased name" );

					return name.toLowerCase();
				}
			}

			return null;
		},

		set: origAttrHooks.set || function( elem, value, name ) {
			if ( jQuery.migrateIsPatchEnabled( "boolean-attributes" ) ) {
				if ( value !== name &&
					( extraBoolAttrValues[ name ] || [] )
						.indexOf( String( value ).toLowerCase() ) === -1
				) {
					if ( value !== false ) {
						migrateWarn( "boolean-attributes",
							"Boolean attribute '" + name +
								"' is not set to its lowercased name" );
					}

					if ( value === false ) {

						// Remove boolean attributes when set to false
						jQuery.removeAttr( elem, name );
					} else {
						elem.setAttribute( name, name );
					}
					return name;
				}
			}
		}
	};
} );

migratePatchFunc( jQuery, "attr", function( elem, name, value ) {
	var nType = elem.nodeType;

	// Fallback to the original method on text, comment and attribute nodes
	// and when attributes are not supported.
	if ( nType === 3 || nType === 8 || nType === 2 ||
			typeof elem.getAttribute === "undefined" ) {
		return oldJQueryAttr.apply( this, arguments );
	}

	if ( value === false && name.toLowerCase().indexOf( "aria-" ) !== 0 &&
			!rbooleans.test( name ) ) {
		migrateWarn( "attr-false",
			"Setting the non-ARIA non-boolean attribute '" + name +
				"' to false" );

		jQuery.attr( elem, name, "false" );
		return;
	}

	return oldJQueryAttr.apply( this, arguments );
}, "attr-false" );

migratePatchFunc( jQuery.fn, "toggleClass", function( state ) {

	// Only deprecating no-args or single boolean arg
	if ( state !== undefined && typeof state !== "boolean" ) {

		return oldToggleClass.apply( this, arguments );
	}

	migrateWarn( "toggleClass-bool", "jQuery.fn.toggleClass( [ boolean ] ) is removed" );

	// Toggle entire class name of each element
	return this.each( function() {
		var className = this.getAttribute && this.getAttribute( "class" ) || "";

		if ( className ) {
			jQuery.data( this, "__className__", className );
		}

		// If the element has a class name or if we're passed `false`,
		// then remove the whole classname (if there was one, the above saved it).
		// Otherwise, bring back whatever was previously saved (if anything),
		// falling back to the empty string if nothing was stored.
		if ( this.setAttribute ) {
			this.setAttribute( "class",
				className || state === false ?
					"" :
					jQuery.data( this, "__className__" ) || ""
			);
		}
	} );
}, "toggleClass-bool" );

function camelCase( string ) {
	return string.replace( /-([a-z])/g, function( _, letter ) {
		return letter.toUpperCase();
	} );
}

// Make `object` inherit from `Object.prototype` via an additional object
// in between; that intermediate object proxies properties
// to `Object.prototype`, warning about their usage first.
function patchProto( object, options ) {
	var i,
		warningId = options.warningId,
		apiName = options.apiName,

		// `Object.prototype` keys are not enumerable so list the
		// official ones here. An alternative would be wrapping
		// objects with a Proxy but that creates additional issues
		// like breaking object identity on subsequent calls.
		objProtoKeys = [
			"__proto__",
			"__defineGetter__",
			"__defineSetter__",
			"__lookupGetter__",
			"__lookupSetter__",
			"hasOwnProperty",
			"isPrototypeOf",
			"propertyIsEnumerable",
			"toLocaleString",
			"toString",
			"valueOf"
		],

		// Use a null prototype at the beginning so that we can define our
		// `__proto__` getter & setter. We'll reset the prototype afterward.
		intermediateObj = Object.create( null );

	for ( i = 0; i < objProtoKeys.length; i++ ) {
		( function( key ) {
			Object.defineProperty( intermediateObj, key, {
				get: function() {
					migrateWarn( warningId,
						"Accessing properties from " + apiName +
						" inherited from Object.prototype is removed" );
					return ( key + "__cache" ) in intermediateObj ?
						intermediateObj[ key + "__cache" ] :
						Object.prototype[ key ];
				},
				set: function( value ) {
					migrateWarn( warningId,
						"Setting properties from " + apiName +
						" inherited from Object.prototype is removed" );
					intermediateObj[ key + "__cache" ] = value;
				}
			} );
		} )( objProtoKeys[ i ] );
	}

	Object.setPrototypeOf( intermediateObj, Object.prototype );
	Object.setPrototypeOf( object, intermediateObj );

	return object;
}

var origFnCss, internalCssNumber,
	ralphaStart = /^[a-z]/,

	// The regex visualized:
	//
	//                         /----------\
	//                        |            |    /-------\
	//                        |  / Top  \  |   |         |
	//         /--- Border ---+-| Right  |-+---+- Width -+---\
	//        |                 | Bottom |                    |
	//        |                  \ Left /                     |
	//        |                                               |
	//        |                              /----------\     |
	//        |          /-------------\    |            |    |- END
	//        |         |               |   |  / Top  \  |    |
	//        |         |  / Margin  \  |   | | Right  | |    |
	//        |---------+-|           |-+---+-| Bottom |-+----|
	//        |            \ Padding /         \ Left /       |
	// BEGIN -|                                               |
	//        |                /---------\                    |
	//        |               |           |                   |
	//        |               |  / Min \  |    / Width  \     |
	//         \--------------+-|       |-+---|          |---/
	//                           \ Max /       \ Height /
	rautoPx = /^(?:Border(?:Top|Right|Bottom|Left)?(?:Width|)|(?:Margin|Padding)?(?:Top|Right|Bottom|Left)?|(?:Min|Max)?(?:Width|Height))$/;

if ( typeof Proxy !== "undefined" ) {
	jQuery.cssProps = new Proxy( jQuery.cssProps || {}, {
		set: function() {
			migrateWarn( "cssProps", "jQuery.cssProps is removed" );
			return Reflect.set.apply( this, arguments );
		}
	} );
}

// `jQuery.cssNumber` is missing in jQuery >=4; fill it with the latest 3.x version:
// https://github.com/jquery/jquery/blob/3.7.1/src/css.js#L216-L246
// This way, number values for the CSS properties below won't start triggering
// Migrate warnings when jQuery gets updated to >=4.0.0 (gh-438).
//
// We need to keep this as a local variable as we need it internally
// in a `jQuery.fn.css` patch and this usage shouldn't warn.
internalCssNumber = {
	animationIterationCount: true,
	aspectRatio: true,
	borderImageSlice: true,
	columnCount: true,
	flexGrow: true,
	flexShrink: true,
	fontWeight: true,
	gridArea: true,
	gridColumn: true,
	gridColumnEnd: true,
	gridColumnStart: true,
	gridRow: true,
	gridRowEnd: true,
	gridRowStart: true,
	lineHeight: true,
	opacity: true,
	order: true,
	orphans: true,
	scale: true,
	widows: true,
	zIndex: true,
	zoom: true,

	// SVG-related
	fillOpacity: true,
	floodOpacity: true,
	stopOpacity: true,
	strokeMiterlimit: true,
	strokeOpacity: true
};

if ( typeof Proxy !== "undefined" ) {
	jQuery.cssNumber = new Proxy( internalCssNumber, {
		get: function() {
			migrateWarn( "css-number", "jQuery.cssNumber is deprecated" );
			return Reflect.get.apply( this, arguments );
		},
		set: function() {
			migrateWarn( "css-number", "jQuery.cssNumber is deprecated" );
			return Reflect.set.apply( this, arguments );
		}
	} );
} else {

	// Support: IE 9-11+
	// IE doesn't support proxies, but we still want to restore the legacy
	// jQuery.cssNumber there.
	jQuery.cssNumber = internalCssNumber;
}

function isAutoPx( prop ) {

	// The first test is used to ensure that:
	// 1. The prop starts with a lowercase letter (as we uppercase it for the second regex).
	// 2. The prop is not empty.
	return ralphaStart.test( prop ) &&
		rautoPx.test( prop[ 0 ].toUpperCase() + prop.slice( 1 ) );
}

origFnCss = jQuery.fn.css;

migratePatchFunc( jQuery.fn, "css", function( name, value ) {
	var camelName,
		origThis = this;

	if ( name && typeof name === "object" && !Array.isArray( name ) ) {
		jQuery.each( name, function( n, v ) {
			jQuery.fn.css.call( origThis, n, v );
		} );
		return this;
	}

	if ( typeof value === "number" ) {
		camelName = camelCase( name );

		// Use `internalCssNumber` to avoid triggering our warnings in this
		// internal check.
		if ( !isAutoPx( camelName ) && !internalCssNumber[ camelName ] ) {
			migrateWarn( "css-number",
				"Auto-appending 'px' to number-typed values " +
					"for jQuery.fn.css( \"" + name + "\", value ) is removed" );
		}
	}

	return origFnCss.apply( this, arguments );
}, "css-number" );

function patchDataProto( original, options ) {
	var warningId = options.warningId,
		apiName = options.apiName,
		isInstanceMethod = options.isInstanceMethod;

	return function apiWithProtoPatched() {
		var result = original.apply( this, arguments );

		if ( arguments.length !== ( isInstanceMethod ? 0 : 1 ) || result === undefined ) {
			return result;
		}

		patchProto( result, {
			warningId: warningId,
			apiName: apiName
		} );

		return result;
	};
}

migratePatchFunc( jQuery, "data",
	patchDataProto( jQuery.data, {
		warningId: "data-null-proto",
		apiName: "jQuery.data()",
		isInstanceMethod: false
	} ),
	"data-null-proto" );
migratePatchFunc( jQuery, "_data",
	patchDataProto( jQuery._data, {
		warningId: "data-null-proto",
		apiName: "jQuery._data()",
		isInstanceMethod: false
	} ),
	"data-null-proto" );
migratePatchFunc( jQuery.fn, "data",
	patchDataProto( jQuery.fn.data, {
		warningId: "data-null-proto",
		apiName: "jQuery.fn.data()",
		isInstanceMethod: true
	} ),
	"data-null-proto" );

// Support jQuery slim which excludes the effects module
if ( jQuery.fx ) {

var intervalValue = jQuery.fx.interval,
	intervalMsg = "jQuery.fx.interval is removed";

// Don't warn if document is hidden, jQuery uses setTimeout (gh-292)
Object.defineProperty( jQuery.fx, "interval", {
	configurable: true,
	enumerable: true,
	get: function() {
		if ( !window.document.hidden ) {
			migrateWarn( "fx-interval", intervalMsg );
		}

		// Only fallback to the default if patch is enabled
		if ( !jQuery.migrateIsPatchEnabled( "fx-interval" ) ) {
			return intervalValue;
		}
		return intervalValue === undefined ? 13 : intervalValue;
	},
	set: function( newValue ) {
		migrateWarn( "fx-interval", intervalMsg );
		intervalValue = newValue;
	}
} );

}

var oldEventAdd = jQuery.event.add;

jQuery.event.props = [];
jQuery.event.fixHooks = {};

migratePatchFunc( jQuery.event, "add", function( elem, types ) {

	// This misses the multiple-types case but that seems awfully rare
	if ( elem === window && types === "load" && window.document.readyState === "complete" ) {
		migrateWarn( "load-after-event",
			"jQuery(window).on('load'...) called after load event occurred" );
	}
	return oldEventAdd.apply( this, arguments );
}, "load-after-event" );

jQuery.each( ( "blur focus focusin focusout resize scroll click dblclick " +
	"mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave " +
	"change select submit keydown keypress keyup contextmenu" ).split( " " ),
function( _i, name ) {

	// Handle event binding
	migratePatchAndInfoFunc( jQuery.fn, name, jQuery.fn[ name ], "shorthand-deprecated-v3",
		"jQuery.fn." + name + "() event shorthand is deprecated" );
} );

migratePatchAndInfoFunc( jQuery.fn, "bind", jQuery.fn.bind,
	"pre-on-methods", "jQuery.fn.bind() is deprecated" );
migratePatchAndInfoFunc( jQuery.fn, "unbind", jQuery.fn.unbind,
	"pre-on-methods", "jQuery.fn.unbind() is deprecated" );
migratePatchAndInfoFunc( jQuery.fn, "delegate", jQuery.fn.delegate,
	"pre-on-methods", "jQuery.fn.delegate() is deprecated" );
migratePatchAndInfoFunc( jQuery.fn, "undelegate", jQuery.fn.undelegate,
	"pre-on-methods", "jQuery.fn.undelegate() is deprecated" );

migratePatchAndInfoFunc( jQuery.fn, "hover", jQuery.fn.hover,
	"hover", "jQuery.fn.hover() is deprecated" );

migrateWarnProp( jQuery.event, "global", {}, "event-global",
	"jQuery.event.global is removed" );

migratePatchProp( jQuery.event, "special",
	patchProto( jQuery.extend( Object.create( null ), jQuery.event.special ), {
		warningId: "event-special-null-proto",
		apiName: "jQuery.event.special"
	} ),
	"event-special-null-proto" );

var rxhtmlTag = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([a-z][^\/\0>\x20\t\r\n\f]*)[^>]*)\/>/gi,
	makeMarkup = function( html ) {
		var doc = window.document.implementation.createHTMLDocument( "" );
		doc.body.innerHTML = html;
		return doc.body && doc.body.innerHTML;
	},
	warnIfChanged = function( html ) {
		var changed = html.replace( rxhtmlTag, "<$1></$2>" );
		if ( changed !== html && makeMarkup( html ) !== makeMarkup( changed ) ) {
			migrateWarn( "self-closed-tags",
				"HTML tags must be properly nested and closed: " + html );
		}
	};

migratePatchFunc( jQuery, "htmlPrefilter", function( html ) {
	warnIfChanged( html );
	return html.replace( rxhtmlTag, "<$1></$2>" );
}, "self-closed-tags" );

// This patch needs to be disabled by default as it re-introduces
// security issues (CVE-2020-11022, CVE-2020-11023).
jQuery.migrateDisablePatches( "self-closed-tags" );

// Support jQuery slim which excludes the deferred module in jQuery 4.0+
if ( jQuery.Deferred ) {

var unpatchedGetStackHookValue,
	oldDeferred = jQuery.Deferred;

migratePatchFunc( jQuery, "Deferred", function( func ) {
	var deferred = oldDeferred(),
		promise = deferred.promise();

	migratePatchAndInfoFunc( deferred, "pipe", deferred.pipe, "deferred-pipe",
		"deferred.pipe() is deprecated" );
	migratePatchAndInfoFunc( promise, "pipe", promise.pipe, "deferred-pipe",
		"deferred.pipe() is deprecated" );

	if ( func ) {
		func.call( deferred, deferred );
	}

	return deferred;
}, "deferred-pipe" );

// Preserve handler of uncaught exceptions in promise chains
jQuery.Deferred.exceptionHook = oldDeferred.exceptionHook;

// Preserve the optional hook to record the error, if defined
jQuery.Deferred.getErrorHook = oldDeferred.getErrorHook;

// We want to mirror jQuery.Deferred.getErrorHook here, so we cannot use
// existing Migrate utils.
Object.defineProperty( jQuery.Deferred, "getStackHook", {
	configurable: true,
	enumerable: true,
	get: function() {
		if ( jQuery.migrateIsPatchEnabled( "deferred-getStackHook" ) ) {
			migrateWarn( "deferred-getStackHook",
				"jQuery.Deferred.getStackHook is removed; use jQuery.Deferred.getErrorHook" );
			return jQuery.Deferred.getErrorHook;
		} else {
			return unpatchedGetStackHookValue;
		}
	},
	set: function( newValue ) {
		if ( jQuery.migrateIsPatchEnabled( "deferred-getStackHook" ) ) {

			// Only warn if `getErrorHook` wasn't set to the same value first.
			if ( jQuery.Deferred.getErrorHook !== newValue ) {
				migrateWarn( "deferred-getStackHook",
					"jQuery.Deferred.getStackHook is removed; use jQuery.Deferred.getErrorHook" );
				jQuery.Deferred.getErrorHook = newValue;
			}
		} else {
			unpatchedGetStackHookValue = newValue;
		}
	}
} );

}

return jQuery;
} );
