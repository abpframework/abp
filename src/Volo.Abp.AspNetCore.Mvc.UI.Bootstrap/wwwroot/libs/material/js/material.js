/*!
 * Daemonite Material v4.0.0 (http://daemonite.github.io/material/)
 * Copyright 2011-2018 Daemon Pty Ltd
 * Licensed under MIT (https://github.com/Daemonite/material/blob/master/LICENSE)
 */

(function (global, factory) {
	typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('jquery')) :
	typeof define === 'function' && define.amd ? define(['exports', 'jquery'], factory) :
	(factory((global.material = {}),global.jQuery));
}(this, (function (exports,$) { 'use strict';

$ = $ && $.hasOwnProperty('default') ? $['default'] : $;

function createCommonjsModule(fn, module) {
	return module = { exports: {} }, fn(module, module.exports), module.exports;
}

var picker = createCommonjsModule(function (module, exports) {
/*!
 * pickadate.js v3.5.6, 2015/04/20
 * By Amsul, http://amsul.ca
 * Hosted on http://amsul.github.io/pickadate.js
 * Licensed under MIT
 */

(function ( factory ) {

    // AMD.
    if ( typeof undefined == 'function' && undefined.amd )
        undefined( 'picker', ['jquery'], factory );

    // Node.js/browserify.
    else module.exports = factory( $ );

}(function( $$$1 ) {

var $window = $$$1( window );
var $document = $$$1( document );
var $html = $$$1( document.documentElement );
var supportsTransitions = document.documentElement.style.transition != null;


/**
 * The picker constructor that creates a blank picker.
 */
function PickerConstructor( ELEMENT, NAME, COMPONENT, OPTIONS ) {

    // If there’s no element, return the picker constructor.
    if ( !ELEMENT ) return PickerConstructor


    var
        IS_DEFAULT_THEME = false,


        // The state of the picker.
        STATE = {
            id: ELEMENT.id || 'P' + Math.abs( ~~(Math.random() * new Date()) )
        },


        // Merge the defaults and options passed.
        SETTINGS = COMPONENT ? $$$1.extend( true, {}, COMPONENT.defaults, OPTIONS ) : OPTIONS || {},


        // Merge the default classes with the settings classes.
        CLASSES = $$$1.extend( {}, PickerConstructor.klasses(), SETTINGS.klass ),


        // The element node wrapper into a jQuery object.
        $ELEMENT = $$$1( ELEMENT ),


        // Pseudo picker constructor.
        PickerInstance = function() {
            return this.start()
        },


        // The picker prototype.
        P = PickerInstance.prototype = {

            constructor: PickerInstance,

            $node: $ELEMENT,


            /**
             * Initialize everything
             */
            start: function() {

                // If it’s already started, do nothing.
                if ( STATE && STATE.start ) return P


                // Update the picker states.
                STATE.methods = {};
                STATE.start = true;
                STATE.open = false;
                STATE.type = ELEMENT.type;


                // Confirm focus state, convert into text input to remove UA stylings,
                // and set as readonly to prevent keyboard popup.
                ELEMENT.autofocus = ELEMENT == getActiveElement();
                ELEMENT.readOnly = !SETTINGS.editable;
                ELEMENT.id = ELEMENT.id || STATE.id;
                if ( ELEMENT.type != 'text' ) {
                    ELEMENT.type = 'text';
                }


                // Create a new picker component with the settings.
                P.component = new COMPONENT(P, SETTINGS);


                // Create the picker root and then prepare it.
                P.$root = $$$1( '<div class="' + CLASSES.picker + '" id="' + ELEMENT.id + '_root" />' );
                prepareElementRoot();


                // Create the picker holder and then prepare it.
                P.$holder = $$$1( createWrappedComponent() ).appendTo( P.$root );
                prepareElementHolder();


                // If there’s a format for the hidden input element, create the element.
                if ( SETTINGS.formatSubmit ) {
                    prepareElementHidden();
                }


                // Prepare the input element.
                prepareElement();


                // Insert the hidden input as specified in the settings.
                if ( SETTINGS.containerHidden ) $$$1( SETTINGS.containerHidden ).append( P._hidden );
                else $ELEMENT.after( P._hidden );


                // Insert the root as specified in the settings.
                if ( SETTINGS.container ) $$$1( SETTINGS.container ).append( P.$root );
                else $ELEMENT.after( P.$root );


                // Bind the default component and settings events.
                P.on({
                    start: P.component.onStart,
                    render: P.component.onRender,
                    stop: P.component.onStop,
                    open: P.component.onOpen,
                    close: P.component.onClose,
                    set: P.component.onSet
                }).on({
                    start: SETTINGS.onStart,
                    render: SETTINGS.onRender,
                    stop: SETTINGS.onStop,
                    open: SETTINGS.onOpen,
                    close: SETTINGS.onClose,
                    set: SETTINGS.onSet
                });


                // Once we’re all set, check the theme in use.
                IS_DEFAULT_THEME = isUsingDefaultTheme( P.$holder[0] );


                // If the element has autofocus, open the picker.
                if ( ELEMENT.autofocus ) {
                    P.open();
                }


                // Trigger queued the “start” and “render” events.
                return P.trigger( 'start' ).trigger( 'render' )
            }, //start


            /**
             * Render a new picker
             */
            render: function( entireComponent ) {

                // Insert a new component holder in the root or box.
                if ( entireComponent ) {
                    P.$holder = $$$1( createWrappedComponent() );
                    prepareElementHolder();
                    P.$root.html( P.$holder );
                }
                else P.$root.find( '.' + CLASSES.box ).html( P.component.nodes( STATE.open ) );

                // Trigger the queued “render” events.
                return P.trigger( 'render' )
            }, //render


            /**
             * Destroy everything
             */
            stop: function() {

                // If it’s already stopped, do nothing.
                if ( !STATE.start ) return P

                // Then close the picker.
                P.close();

                // Remove the hidden field.
                if ( P._hidden ) {
                    P._hidden.parentNode.removeChild( P._hidden );
                }

                // Remove the root.
                P.$root.remove();

                // Remove the input class, remove the stored data, and unbind
                // the events (after a tick for IE - see `P.close`).
                $ELEMENT.removeClass( CLASSES.input ).removeData( NAME );
                setTimeout( function() {
                    $ELEMENT.off( '.' + STATE.id );
                }, 0);

                // Restore the element state
                ELEMENT.type = STATE.type;
                ELEMENT.readOnly = false;

                // Trigger the queued “stop” events.
                P.trigger( 'stop' );

                // Reset the picker states.
                STATE.methods = {};
                STATE.start = false;

                return P
            }, //stop


            /**
             * Open up the picker
             */
            open: function( dontGiveFocus ) {

                // If it’s already open, do nothing.
                if ( STATE.open ) return P

                // Add the “active” class.
                $ELEMENT.addClass( CLASSES.active );
                aria( ELEMENT, 'expanded', true );

                // * A Firefox bug, when `html` has `overflow:hidden`, results in
                //   killing transitions :(. So add the “opened” state on the next tick.
                //   Bug: https://bugzilla.mozilla.org/show_bug.cgi?id=625289
                setTimeout( function() {

                    // Add the “opened” class to the picker root.
                    P.$root.addClass( CLASSES.opened );
                    aria( P.$root[0], 'hidden', false );

                }, 0 );

                // If we have to give focus, bind the element and doc events.
                if ( dontGiveFocus !== false ) {

                    // Set it as open.
                    STATE.open = true;

                    // Prevent the page from scrolling.
                    if ( IS_DEFAULT_THEME ) {
                        $html.
                            css( 'overflow', 'hidden' ).
                            css( 'padding-right', '+=' + getScrollbarWidth() );
                    }

                    // Pass focus to the root element’s jQuery object.
                    focusPickerOnceOpened();

                    // Bind the document events.
                    $document.on( 'click.' + STATE.id + ' focusin.' + STATE.id, function( event ) {

                        var target = event.target;

                        // If the target of the event is not the element, close the picker picker.
                        // * Don’t worry about clicks or focusins on the root because those don’t bubble up.
                        //   Also, for Firefox, a click on an `option` element bubbles up directly
                        //   to the doc. So make sure the target wasn't the doc.
                        // * In Firefox stopPropagation() doesn’t prevent right-click events from bubbling,
                        //   which causes the picker to unexpectedly close when right-clicking it. So make
                        //   sure the event wasn’t a right-click.
                        if ( target != ELEMENT && target != document && event.which != 3 ) {

                            // If the target was the holder that covers the screen,
                            // keep the element focused to maintain tabindex.
                            P.close( target === P.$holder[0] );
                        }

                    }).on( 'keydown.' + STATE.id, function( event ) {

                        var
                            // Get the keycode.
                            keycode = event.keyCode,

                            // Translate that to a selection change.
                            keycodeToMove = P.component.key[ keycode ],

                            // Grab the target.
                            target = event.target;


                        // On escape, close the picker and give focus.
                        if ( keycode == 27 ) {
                            P.close( true );
                        }


                        // Check if there is a key movement or “enter” keypress on the element.
                        else if ( target == P.$holder[0] && ( keycodeToMove || keycode == 13 ) ) {

                            // Prevent the default action to stop page movement.
                            event.preventDefault();

                            // Trigger the key movement action.
                            if ( keycodeToMove ) {
                                PickerConstructor._.trigger( P.component.key.go, P, [ PickerConstructor._.trigger( keycodeToMove ) ] );
                            }

                            // On “enter”, if the highlighted item isn’t disabled, set the value and close.
                            else if ( !P.$root.find( '.' + CLASSES.highlighted ).hasClass( CLASSES.disabled ) ) {
                                P.set( 'select', P.component.item.highlight );
                                if ( SETTINGS.closeOnSelect ) {
                                    P.close( true );
                                }
                            }
                        }


                        // If the target is within the root and “enter” is pressed,
                        // prevent the default action and trigger a click on the target instead.
                        else if ( $$$1.contains( P.$root[0], target ) && keycode == 13 ) {
                            event.preventDefault();
                            target.click();
                        }
                    });
                }

                // Trigger the queued “open” events.
                return P.trigger( 'open' )
            }, //open


            /**
             * Close the picker
             */
            close: function( giveFocus ) {

                // If we need to give focus, do it before changing states.
                if ( giveFocus ) {
                    if ( SETTINGS.editable ) {
                        ELEMENT.focus();
                    }
                    else {
                        // ....ah yes! It would’ve been incomplete without a crazy workaround for IE :|
                        // The focus is triggered *after* the close has completed - causing it
                        // to open again. So unbind and rebind the event at the next tick.
                        P.$holder.off( 'focus.toOpen' ).focus();
                        setTimeout( function() {
                            P.$holder.on( 'focus.toOpen', handleFocusToOpenEvent );
                        }, 0 );
                    }
                }

                // Remove the “active” class.
                $ELEMENT.removeClass( CLASSES.active );
                aria( ELEMENT, 'expanded', false );

                // * A Firefox bug, when `html` has `overflow:hidden`, results in
                //   killing transitions :(. So remove the “opened” state on the next tick.
                //   Bug: https://bugzilla.mozilla.org/show_bug.cgi?id=625289
                setTimeout( function() {

                    // Remove the “opened” and “focused” class from the picker root.
                    P.$root.removeClass( CLASSES.opened + ' ' + CLASSES.focused );
                    aria( P.$root[0], 'hidden', true );

                }, 0 );

                // If it’s already closed, do nothing more.
                if ( !STATE.open ) return P

                // Set it as closed.
                STATE.open = false;

                // Allow the page to scroll.
                if ( IS_DEFAULT_THEME ) {
                    $html.
                        css( 'overflow', '' ).
                        css( 'padding-right', '-=' + getScrollbarWidth() );
                }

                // Unbind the document events.
                $document.off( '.' + STATE.id );

                // Trigger the queued “close” events.
                return P.trigger( 'close' )
            }, //close


            /**
             * Clear the values
             */
            clear: function( options ) {
                return P.set( 'clear', null, options )
            }, //clear


            /**
             * Set something
             */
            set: function( thing, value, options ) {

                var thingItem, thingValue,
                    thingIsObject = $$$1.isPlainObject( thing ),
                    thingObject = thingIsObject ? thing : {};

                // Make sure we have usable options.
                options = thingIsObject && $$$1.isPlainObject( value ) ? value : options || {};

                if ( thing ) {

                    // If the thing isn’t an object, make it one.
                    if ( !thingIsObject ) {
                        thingObject[ thing ] = value;
                    }

                    // Go through the things of items to set.
                    for ( thingItem in thingObject ) {

                        // Grab the value of the thing.
                        thingValue = thingObject[ thingItem ];

                        // First, if the item exists and there’s a value, set it.
                        if ( thingItem in P.component.item ) {
                            if ( thingValue === undefined ) thingValue = null;
                            P.component.set( thingItem, thingValue, options );
                        }

                        // Then, check to update the element value and broadcast a change.
                        if ( thingItem == 'select' || thingItem == 'clear' ) {
                            $ELEMENT.
                                val( thingItem == 'clear' ? '' : P.get( thingItem, SETTINGS.format ) ).
                                trigger( 'change' );
                        }
                    }

                    // Render a new picker.
                    P.render();
                }

                // When the method isn’t muted, trigger queued “set” events and pass the `thingObject`.
                return options.muted ? P : P.trigger( 'set', thingObject )
            }, //set


            /**
             * Get something
             */
            get: function( thing, format ) {

                // Make sure there’s something to get.
                thing = thing || 'value';

                // If a picker state exists, return that.
                if ( STATE[ thing ] != null ) {
                    return STATE[ thing ]
                }

                // Return the submission value, if that.
                if ( thing == 'valueSubmit' ) {
                    if ( P._hidden ) {
                        return P._hidden.value
                    }
                    thing = 'value';
                }

                // Return the value, if that.
                if ( thing == 'value' ) {
                    return ELEMENT.value
                }

                // Check if a component item exists, return that.
                if ( thing in P.component.item ) {
                    if ( typeof format == 'string' ) {
                        var thingValue = P.component.get( thing );
                        return thingValue ?
                            PickerConstructor._.trigger(
                                P.component.formats.toString,
                                P.component,
                                [ format, thingValue ]
                            ) : ''
                    }
                    return P.component.get( thing )
                }
            }, //get



            /**
             * Bind events on the things.
             */
            on: function( thing, method, internal ) {

                var thingName, thingMethod,
                    thingIsObject = $$$1.isPlainObject( thing ),
                    thingObject = thingIsObject ? thing : {};

                if ( thing ) {

                    // If the thing isn’t an object, make it one.
                    if ( !thingIsObject ) {
                        thingObject[ thing ] = method;
                    }

                    // Go through the things to bind to.
                    for ( thingName in thingObject ) {

                        // Grab the method of the thing.
                        thingMethod = thingObject[ thingName ];

                        // If it was an internal binding, prefix it.
                        if ( internal ) {
                            thingName = '_' + thingName;
                        }

                        // Make sure the thing methods collection exists.
                        STATE.methods[ thingName ] = STATE.methods[ thingName ] || [];

                        // Add the method to the relative method collection.
                        STATE.methods[ thingName ].push( thingMethod );
                    }
                }

                return P
            }, //on



            /**
             * Unbind events on the things.
             */
            off: function() {
                var i, thingName,
                    names = arguments;
                for ( i = 0, namesCount = names.length; i < namesCount; i += 1 ) {
                    thingName = names[i];
                    if ( thingName in STATE.methods ) {
                        delete STATE.methods[thingName];
                    }
                }
                return P
            },


            /**
             * Fire off method events.
             */
            trigger: function( name, data ) {
                var _trigger = function( name ) {
                    var methodList = STATE.methods[ name ];
                    if ( methodList ) {
                        methodList.map( function( method ) {
                            PickerConstructor._.trigger( method, P, [ data ] );
                        });
                    }
                };
                _trigger( '_' + name );
                _trigger( name );
                return P
            } //trigger
        }; //PickerInstance.prototype


    /**
     * Wrap the picker holder components together.
     */
    function createWrappedComponent() {

        // Create a picker wrapper holder
        return PickerConstructor._.node( 'div',

            // Create a picker wrapper node
            PickerConstructor._.node( 'div',

                // Create a picker frame
                PickerConstructor._.node( 'div',

                    // Create a picker box node
                    PickerConstructor._.node( 'div',

                        // Create the components nodes.
                        P.component.nodes( STATE.open ),

                        // The picker box class
                        CLASSES.box
                    ),

                    // Picker wrap class
                    CLASSES.wrap
                ),

                // Picker frame class
                CLASSES.frame
            ),

            // Picker holder class
            CLASSES.holder,

            'tabindex="-1"'
        ) //endreturn
    } //createWrappedComponent



    /**
     * Prepare the input element with all bindings.
     */
    function prepareElement() {

        $ELEMENT.

            // Store the picker data by component name.
            data(NAME, P).

            // Add the “input” class name.
            addClass(CLASSES.input).

            // If there’s a `data-value`, update the value of the element.
            val( $ELEMENT.data('value') ?
                P.get('select', SETTINGS.format) :
                ELEMENT.value
            );


        // Only bind keydown events if the element isn’t editable.
        if ( !SETTINGS.editable ) {

            $ELEMENT.

                // On focus/click, open the picker.
                on( 'focus.' + STATE.id + ' click.' + STATE.id, function(event) {
                    event.preventDefault();
                    P.open();
                }).

                // Handle keyboard event based on the picker being opened or not.
                on( 'keydown.' + STATE.id, handleKeydownEvent );
        }


        // Update the aria attributes.
        aria(ELEMENT, {
            haspopup: true,
            expanded: false,
            readonly: false,
            owns: ELEMENT.id + '_root'
        });
    }


    /**
     * Prepare the root picker element with all bindings.
     */
    function prepareElementRoot() {
        aria( P.$root[0], 'hidden', true );
    }


     /**
      * Prepare the holder picker element with all bindings.
      */
    function prepareElementHolder() {

        P.$holder.

            on({

                // For iOS8.
                keydown: handleKeydownEvent,

                'focus.toOpen': handleFocusToOpenEvent,

                blur: function() {
                    // Remove the “target” class.
                    $ELEMENT.removeClass( CLASSES.target );
                },

                // When something within the holder is focused, stop from bubbling
                // to the doc and remove the “focused” state from the root.
                focusin: function( event ) {
                    P.$root.removeClass( CLASSES.focused );
                    event.stopPropagation();
                },

                // When something within the holder is clicked, stop it
                // from bubbling to the doc.
                'mousedown click': function( event ) {

                    var target = event.target;

                    // Make sure the target isn’t the root holder so it can bubble up.
                    if ( target != P.$holder[0] ) {

                        event.stopPropagation();

                        // * For mousedown events, cancel the default action in order to
                        //   prevent cases where focus is shifted onto external elements
                        //   when using things like jQuery mobile or MagnificPopup (ref: #249 & #120).
                        //   Also, for Firefox, don’t prevent action on the `option` element.
                        if ( event.type == 'mousedown' && !$$$1( target ).is( 'input, select, textarea, button, option' )) {

                            event.preventDefault();

                            // Re-focus onto the holder so that users can click away
                            // from elements focused within the picker.
                            P.$holder[0].focus();
                        }
                    }
                }

            }).

            // If there’s a click on an actionable element, carry out the actions.
            on( 'click', '[data-pick], [data-nav], [data-clear], [data-close]', function() {

                var $target = $$$1( this ),
                    targetData = $target.data(),
                    targetDisabled = $target.hasClass( CLASSES.navDisabled ) || $target.hasClass( CLASSES.disabled ),

                    // * For IE, non-focusable elements can be active elements as well
                    //   (http://stackoverflow.com/a/2684561).
                    activeElement = getActiveElement();
                    activeElement = activeElement && ( activeElement.type || activeElement.href );

                // If it’s disabled or nothing inside is actively focused, re-focus the element.
                if ( targetDisabled || activeElement && !$$$1.contains( P.$root[0], activeElement ) ) {
                    P.$holder[0].focus();
                }

                // If something is superficially changed, update the `highlight` based on the `nav`.
                if ( !targetDisabled && targetData.nav ) {
                    P.set( 'highlight', P.component.item.highlight, { nav: targetData.nav } );
                }

                // If something is picked, set `select` then close with focus.
                else if ( !targetDisabled && 'pick' in targetData ) {
                    P.set( 'select', targetData.pick );
                    if ( SETTINGS.closeOnSelect ) {
                        P.close( true );
                    }
                }

                // If a “clear” button is pressed, empty the values and close with focus.
                else if ( targetData.clear ) {
                    P.clear();
                    if ( SETTINGS.closeOnClear ) {
                        P.close( true );
                    }
                }

                else if ( targetData.close ) {
                    P.close( true );
                }

            }); //P.$holder

    }


     /**
      * Prepare the hidden input element along with all bindings.
      */
    function prepareElementHidden() {

        var name;

        if ( SETTINGS.hiddenName === true ) {
            name = ELEMENT.name;
            ELEMENT.name = '';
        }
        else {
            name = [
                typeof SETTINGS.hiddenPrefix == 'string' ? SETTINGS.hiddenPrefix : '',
                typeof SETTINGS.hiddenSuffix == 'string' ? SETTINGS.hiddenSuffix : '_submit'
            ];
            name = name[0] + ELEMENT.name + name[1];
        }

        P._hidden = $$$1(
            '<input ' +
            'type=hidden ' +

            // Create the name using the original input’s with a prefix and suffix.
            'name="' + name + '"' +

            // If the element has a value, set the hidden value as well.
            (
                $ELEMENT.data('value') || ELEMENT.value ?
                    ' value="' + P.get('select', SETTINGS.formatSubmit) + '"' :
                    ''
            ) +
            '>'
        )[0];

        $ELEMENT.

            // If the value changes, update the hidden input with the correct format.
            on('change.' + STATE.id, function() {
                P._hidden.value = ELEMENT.value ?
                    P.get('select', SETTINGS.formatSubmit) :
                    '';
            });
    }


    // Wait for transitions to end before focusing the holder. Otherwise, while
    // using the `container` option, the view jumps to the container.
    function focusPickerOnceOpened() {

        if (IS_DEFAULT_THEME && supportsTransitions) {
            P.$holder.find('.' + CLASSES.frame).one('transitionend', function() {
                P.$holder[0].focus();
            });
        }
        else {
            P.$holder[0].focus();
        }
    }


    function handleFocusToOpenEvent(event) {

        // Stop the event from propagating to the doc.
        event.stopPropagation();

        // Add the “target” class.
        $ELEMENT.addClass( CLASSES.target );

        // Add the “focused” class to the root.
        P.$root.addClass( CLASSES.focused );

        // And then finally open the picker.
        P.open();
    }


    // For iOS8.
    function handleKeydownEvent( event ) {

        var keycode = event.keyCode,

            // Check if one of the delete keys was pressed.
            isKeycodeDelete = /^(8|46)$/.test(keycode);

        // For some reason IE clears the input value on “escape”.
        if ( keycode == 27 ) {
            P.close( true );
            return false
        }

        // Check if `space` or `delete` was pressed or the picker is closed with a key movement.
        if ( keycode == 32 || isKeycodeDelete || !STATE.open && P.component.key[keycode] ) {

            // Prevent it from moving the page and bubbling to doc.
            event.preventDefault();
            event.stopPropagation();

            // If `delete` was pressed, clear the values and close the picker.
            // Otherwise open the picker.
            if ( isKeycodeDelete ) { P.clear().close(); }
            else { P.open(); }
        }
    }


    // Return a new picker instance.
    return new PickerInstance()
} //PickerConstructor



/**
 * The default classes and prefix to use for the HTML classes.
 */
PickerConstructor.klasses = function( prefix ) {
    prefix = prefix || 'picker';
    return {

        picker: prefix,
        opened: prefix + '--opened',
        focused: prefix + '--focused',

        input: prefix + '__input',
        active: prefix + '__input--active',
        target: prefix + '__input--target',

        holder: prefix + '__holder',

        frame: prefix + '__frame',
        wrap: prefix + '__wrap',

        box: prefix + '__box'
    }
}; //PickerConstructor.klasses



/**
 * Check if the default theme is being used.
 */
function isUsingDefaultTheme( element ) {

    var theme,
        prop = 'position';

    // For IE.
    if ( element.currentStyle ) {
        theme = element.currentStyle[prop];
    }

    // For normal browsers.
    else if ( window.getComputedStyle ) {
        theme = getComputedStyle( element )[prop];
    }

    return theme == 'fixed'
}



/**
 * Get the width of the browser’s scrollbar.
 * Taken from: https://github.com/VodkaBears/Remodal/blob/master/src/jquery.remodal.js
 */
function getScrollbarWidth() {

    if ( $html.height() <= $window.height() ) {
        return 0
    }

    var $outer = $$$1( '<div style="visibility:hidden;width:100px" />' ).
        appendTo( 'body' );

    // Get the width without scrollbars.
    var widthWithoutScroll = $outer[0].offsetWidth;

    // Force adding scrollbars.
    $outer.css( 'overflow', 'scroll' );

    // Add the inner div.
    var $inner = $$$1( '<div style="width:100%" />' ).appendTo( $outer );

    // Get the width with scrollbars.
    var widthWithScroll = $inner[0].offsetWidth;

    // Remove the divs.
    $outer.remove();

    // Return the difference between the widths.
    return widthWithoutScroll - widthWithScroll
}



/**
 * PickerConstructor helper methods.
 */
PickerConstructor._ = {

    /**
     * Create a group of nodes. Expects:
     * `
        {
            min:    {Integer},
            max:    {Integer},
            i:      {Integer},
            node:   {String},
            item:   {Function}
        }
     * `
     */
    group: function( groupObject ) {

        var
            // Scope for the looped object
            loopObjectScope,

            // Create the nodes list
            nodesList = '',

            // The counter starts from the `min`
            counter = PickerConstructor._.trigger( groupObject.min, groupObject );


        // Loop from the `min` to `max`, incrementing by `i`
        for ( ; counter <= PickerConstructor._.trigger( groupObject.max, groupObject, [ counter ] ); counter += groupObject.i ) {

            // Trigger the `item` function within scope of the object
            loopObjectScope = PickerConstructor._.trigger( groupObject.item, groupObject, [ counter ] );

            // Splice the subgroup and create nodes out of the sub nodes
            nodesList += PickerConstructor._.node(
                groupObject.node,
                loopObjectScope[ 0 ],   // the node
                loopObjectScope[ 1 ],   // the classes
                loopObjectScope[ 2 ]    // the attributes
            );
        }

        // Return the list of nodes
        return nodesList
    }, //group


    /**
     * Create a dom node string
     */
    node: function( wrapper, item, klass, attribute ) {

        // If the item is false-y, just return an empty string
        if ( !item ) return ''

        // If the item is an array, do a join
        item = $$$1.isArray( item ) ? item.join( '' ) : item;

        // Check for the class
        klass = klass ? ' class="' + klass + '"' : '';

        // Check for any attributes
        attribute = attribute ? ' ' + attribute : '';

        // Return the wrapped item
        return '<' + wrapper + klass + attribute + '>' + item + '</' + wrapper + '>'
    }, //node


    /**
     * Lead numbers below 10 with a zero.
     */
    lead: function( number ) {
        return ( number < 10 ? '0': '' ) + number
    },


    /**
     * Trigger a function otherwise return the value.
     */
    trigger: function( callback, scope, args ) {
        return typeof callback == 'function' ? callback.apply( scope, args || [] ) : callback
    },


    /**
     * If the second character is a digit, length is 2 otherwise 1.
     */
    digits: function( string ) {
        return ( /\d/ ).test( string[ 1 ] ) ? 2 : 1
    },


    /**
     * Tell if something is a date object.
     */
    isDate: function( value ) {
        return {}.toString.call( value ).indexOf( 'Date' ) > -1 && this.isInteger( value.getDate() )
    },


    /**
     * Tell if something is an integer.
     */
    isInteger: function( value ) {
        return {}.toString.call( value ).indexOf( 'Number' ) > -1 && value % 1 === 0
    },


    /**
     * Create ARIA attribute strings.
     */
    ariaAttr: ariaAttr
}; //PickerConstructor._



/**
 * Extend the picker with a component and defaults.
 */
PickerConstructor.extend = function( name, Component ) {

    // Extend jQuery.
    $$$1.fn[ name ] = function( options, action ) {

        // Grab the component data.
        var componentData = this.data( name );

        // If the picker is requested, return the data object.
        if ( options == 'picker' ) {
            return componentData
        }

        // If the component data exists and `options` is a string, carry out the action.
        if ( componentData && typeof options == 'string' ) {
            return PickerConstructor._.trigger( componentData[ options ], componentData, [ action ] )
        }

        // Otherwise go through each matched element and if the component
        // doesn’t exist, create a new picker using `this` element
        // and merging the defaults and options with a deep copy.
        return this.each( function() {
            var $this = $$$1( this );
            if ( !$this.data( name ) ) {
                new PickerConstructor( this, name, Component, options );
            }
        })
    };

    // Set the defaults.
    $$$1.fn[ name ].defaults = Component.defaults;
}; //PickerConstructor.extend



function aria(element, attribute, value) {
    if ( $$$1.isPlainObject(attribute) ) {
        for ( var key in attribute ) {
            ariaSet(element, key, attribute[key]);
        }
    }
    else {
        ariaSet(element, attribute, value);
    }
}
function ariaSet(element, attribute, value) {
    element.setAttribute(
        (attribute == 'role' ? '' : 'aria-') + attribute,
        value
    );
}
function ariaAttr(attribute, data) {
    if ( !$$$1.isPlainObject(attribute) ) {
        attribute = { attribute: data };
    }
    data = '';
    for ( var key in attribute ) {
        var attr = (key == 'role' ? '' : 'aria-') + key,
            attrVal = attribute[key];
        data += attrVal == null ? '' : attr + '="' + attribute[key] + '"';
    }
    return data
}

// IE8 bug throws an error for activeElements within iframes.
function getActiveElement() {
    try {
        return document.activeElement
    } catch ( err ) { }
}



// Expose the picker constructor.
return PickerConstructor


}));
});

var picker_date = createCommonjsModule(function (module, exports) {
/*!
 * Date picker for pickadate.js v3.5.6
 * http://amsul.github.io/pickadate.js/date.htm
 */

(function ( factory ) {

    // AMD.
    if ( typeof undefined == 'function' && undefined.amd )
        undefined( ['picker', 'jquery'], factory );

    // Node.js/browserify.
    else module.exports = factory( picker, $ );

}(function( Picker, $$$1 ) {


/**
 * Globals and constants
 */
var DAYS_IN_WEEK = 7,
    WEEKS_IN_CALENDAR = 6,
    _ = Picker._;



/**
 * The date picker constructor
 */
function DatePicker( picker$$1, settings ) {

    var calendar = this,
        element = picker$$1.$node[ 0 ],
        elementValue = element.value,
        elementDataValue = picker$$1.$node.data( 'value' ),
        valueString = elementDataValue || elementValue,
        formatString = elementDataValue ? settings.formatSubmit : settings.format,
        isRTL = function() {

            return element.currentStyle ?

                // For IE.
                element.currentStyle.direction == 'rtl' :

                // For normal browsers.
                getComputedStyle( picker$$1.$root[0] ).direction == 'rtl'
        };

    calendar.settings = settings;
    calendar.$node = picker$$1.$node;

    // The queue of methods that will be used to build item objects.
    calendar.queue = {
        min: 'measure create',
        max: 'measure create',
        now: 'now create',
        select: 'parse create validate',
        highlight: 'parse navigate create validate',
        view: 'parse create validate viewset',
        disable: 'deactivate',
        enable: 'activate'
    };

    // The component's item object.
    calendar.item = {};

    calendar.item.clear = null;
    calendar.item.disable = ( settings.disable || [] ).slice( 0 );
    calendar.item.enable = -(function( collectionDisabled ) {
        return collectionDisabled[ 0 ] === true ? collectionDisabled.shift() : -1
    })( calendar.item.disable );

    calendar.
        set( 'min', settings.min ).
        set( 'max', settings.max ).
        set( 'now' );

    // When there’s a value, set the `select`, which in turn
    // also sets the `highlight` and `view`.
    if ( valueString ) {
        calendar.set( 'select', valueString, {
            format: formatString,
            defaultValue: true
        });
    }

    // If there’s no value, default to highlighting “today”.
    else {
        calendar.
            set( 'select', null ).
            set( 'highlight', calendar.item.now );
    }


    // The keycode to movement mapping.
    calendar.key = {
        40: 7, // Down
        38: -7, // Up
        39: function() { return isRTL() ? -1 : 1 }, // Right
        37: function() { return isRTL() ? 1 : -1 }, // Left
        go: function( timeChange ) {
            var highlightedObject = calendar.item.highlight,
                targetDate = new Date( highlightedObject.year, highlightedObject.month, highlightedObject.date + timeChange );
            calendar.set(
                'highlight',
                targetDate,
                { interval: timeChange }
            );
            this.render();
        }
    };


    // Bind some picker events.
    picker$$1.
        on( 'render', function() {
            picker$$1.$root.find( '.' + settings.klass.selectMonth ).on( 'change', function() {
                var value = this.value;
                if ( value ) {
                    picker$$1.set( 'highlight', [ picker$$1.get( 'view' ).year, value, picker$$1.get( 'highlight' ).date ] );
                    picker$$1.$root.find( '.' + settings.klass.selectMonth ).trigger( 'focus' );
                }
            });
            picker$$1.$root.find( '.' + settings.klass.selectYear ).on( 'change', function() {
                var value = this.value;
                if ( value ) {
                    picker$$1.set( 'highlight', [ value, picker$$1.get( 'view' ).month, picker$$1.get( 'highlight' ).date ] );
                    picker$$1.$root.find( '.' + settings.klass.selectYear ).trigger( 'focus' );
                }
            });
        }, 1 ).
        on( 'open', function() {
            var includeToday = '';
            if ( calendar.disabled( calendar.get('now') ) ) {
                includeToday = ':not(.' + settings.klass.buttonToday + ')';
            }
            picker$$1.$root.find( 'button' + includeToday + ', select' ).attr( 'disabled', false );
        }, 1 ).
        on( 'close', function() {
            picker$$1.$root.find( 'button, select' ).attr( 'disabled', true );
        }, 1 );

} //DatePicker


/**
 * Set a datepicker item object.
 */
DatePicker.prototype.set = function( type, value, options ) {

    var calendar = this,
        calendarItem = calendar.item;

    // If the value is `null` just set it immediately.
    if ( value === null ) {
        if ( type == 'clear' ) type = 'select';
        calendarItem[ type ] = value;
        return calendar
    }

    // Otherwise go through the queue of methods, and invoke the functions.
    // Update this as the time unit, and set the final value as this item.
    // * In the case of `enable`, keep the queue but set `disable` instead.
    //   And in the case of `flip`, keep the queue but set `enable` instead.
    calendarItem[ ( type == 'enable' ? 'disable' : type == 'flip' ? 'enable' : type ) ] = calendar.queue[ type ].split( ' ' ).map( function( method ) {
        value = calendar[ method ]( type, value, options );
        return value
    }).pop();

    // Check if we need to cascade through more updates.
    if ( type == 'select' ) {
        calendar.set( 'highlight', calendarItem.select, options );
    }
    else if ( type == 'highlight' ) {
        calendar.set( 'view', calendarItem.highlight, options );
    }
    else if ( type.match( /^(flip|min|max|disable|enable)$/ ) ) {
        if ( calendarItem.select && calendar.disabled( calendarItem.select ) ) {
            calendar.set( 'select', calendarItem.select, options );
        }
        if ( calendarItem.highlight && calendar.disabled( calendarItem.highlight ) ) {
            calendar.set( 'highlight', calendarItem.highlight, options );
        }
    }

    return calendar
}; //DatePicker.prototype.set


/**
 * Get a datepicker item object.
 */
DatePicker.prototype.get = function( type ) {
    return this.item[ type ]
}; //DatePicker.prototype.get


/**
 * Create a picker date object.
 */
DatePicker.prototype.create = function( type, value, options ) {

    var isInfiniteValue,
        calendar = this;

    // If there’s no value, use the type as the value.
    value = value === undefined ? type : value;


    // If it’s infinity, update the value.
    if ( value == -Infinity || value == Infinity ) {
        isInfiniteValue = value;
    }

    // If it’s an object, use the native date object.
    else if ( $$$1.isPlainObject( value ) && _.isInteger( value.pick ) ) {
        value = value.obj;
    }

    // If it’s an array, convert it into a date and make sure
    // that it’s a valid date – otherwise default to today.
    else if ( $$$1.isArray( value ) ) {
        value = new Date( value[ 0 ], value[ 1 ], value[ 2 ] );
        value = _.isDate( value ) ? value : calendar.create().obj;
    }

    // If it’s a number or date object, make a normalized date.
    else if ( _.isInteger( value ) || _.isDate( value ) ) {
        value = calendar.normalize( new Date( value ), options );
    }

    // If it’s a literal true or any other case, set it to now.
    else /*if ( value === true )*/ {
        value = calendar.now( type, value, options );
    }

    // Return the compiled object.
    return {
        year: isInfiniteValue || value.getFullYear(),
        month: isInfiniteValue || value.getMonth(),
        date: isInfiniteValue || value.getDate(),
        day: isInfiniteValue || value.getDay(),
        obj: isInfiniteValue || value,
        pick: isInfiniteValue || value.getTime()
    }
}; //DatePicker.prototype.create


/**
 * Create a range limit object using an array, date object,
 * literal “true”, or integer relative to another time.
 */
DatePicker.prototype.createRange = function( from, to ) {

    var calendar = this,
        createDate = function( date ) {
            if ( date === true || $$$1.isArray( date ) || _.isDate( date ) ) {
                return calendar.create( date )
            }
            return date
        };

    // Create objects if possible.
    if ( !_.isInteger( from ) ) {
        from = createDate( from );
    }
    if ( !_.isInteger( to ) ) {
        to = createDate( to );
    }

    // Create relative dates.
    if ( _.isInteger( from ) && $$$1.isPlainObject( to ) ) {
        from = [ to.year, to.month, to.date + from ];
    }
    else if ( _.isInteger( to ) && $$$1.isPlainObject( from ) ) {
        to = [ from.year, from.month, from.date + to ];
    }

    return {
        from: createDate( from ),
        to: createDate( to )
    }
}; //DatePicker.prototype.createRange


/**
 * Check if a date unit falls within a date range object.
 */
DatePicker.prototype.withinRange = function( range, dateUnit ) {
    range = this.createRange(range.from, range.to);
    return dateUnit.pick >= range.from.pick && dateUnit.pick <= range.to.pick
};


/**
 * Check if two date range objects overlap.
 */
DatePicker.prototype.overlapRanges = function( one, two ) {

    var calendar = this;

    // Convert the ranges into comparable dates.
    one = calendar.createRange( one.from, one.to );
    two = calendar.createRange( two.from, two.to );

    return calendar.withinRange( one, two.from ) || calendar.withinRange( one, two.to ) ||
        calendar.withinRange( two, one.from ) || calendar.withinRange( two, one.to )
};


/**
 * Get the date today.
 */
DatePicker.prototype.now = function( type, value, options ) {
    value = new Date();
    if ( options && options.rel ) {
        value.setDate( value.getDate() + options.rel );
    }
    return this.normalize( value, options )
};


/**
 * Navigate to next/prev month.
 */
DatePicker.prototype.navigate = function( type, value, options ) {

    var targetDateObject,
        targetYear,
        targetMonth,
        targetDate,
        isTargetArray = $$$1.isArray( value ),
        isTargetObject = $$$1.isPlainObject( value ),
        viewsetObject = this.item.view;/*,
        safety = 100*/


    if ( isTargetArray || isTargetObject ) {

        if ( isTargetObject ) {
            targetYear = value.year;
            targetMonth = value.month;
            targetDate = value.date;
        }
        else {
            targetYear = +value[0];
            targetMonth = +value[1];
            targetDate = +value[2];
        }

        // If we’re navigating months but the view is in a different
        // month, navigate to the view’s year and month.
        if ( options && options.nav && viewsetObject && viewsetObject.month !== targetMonth ) {
            targetYear = viewsetObject.year;
            targetMonth = viewsetObject.month;
        }

        // Figure out the expected target year and month.
        targetDateObject = new Date( targetYear, targetMonth + ( options && options.nav ? options.nav : 0 ), 1 );
        targetYear = targetDateObject.getFullYear();
        targetMonth = targetDateObject.getMonth();

        // If the month we’re going to doesn’t have enough days,
        // keep decreasing the date until we reach the month’s last date.
        while ( /*safety &&*/ new Date( targetYear, targetMonth, targetDate ).getMonth() !== targetMonth ) {
            targetDate -= 1;
            /*safety -= 1
            if ( !safety ) {
                throw 'Fell into an infinite loop while navigating to ' + new Date( targetYear, targetMonth, targetDate ) + '.'
            }*/
        }

        value = [ targetYear, targetMonth, targetDate ];
    }

    return value
}; //DatePicker.prototype.navigate


/**
 * Normalize a date by setting the hours to midnight.
 */
DatePicker.prototype.normalize = function( value/*, options*/ ) {
    value.setHours( 0, 0, 0, 0 );
    return value
};


/**
 * Measure the range of dates.
 */
DatePicker.prototype.measure = function( type, value/*, options*/ ) {

    var calendar = this;

    // If it’s anything false-y, remove the limits.
    if ( !value ) {
        value = type == 'min' ? -Infinity : Infinity;
    }

    // If it’s a string, parse it.
    else if ( typeof value == 'string' ) {
        value = calendar.parse( type, value );
    }

    // If it's an integer, get a date relative to today.
    else if ( _.isInteger( value ) ) {
        value = calendar.now( type, value, { rel: value } );
    }

    return value
}; ///DatePicker.prototype.measure


/**
 * Create a viewset object based on navigation.
 */
DatePicker.prototype.viewset = function( type, dateObject/*, options*/ ) {
    return this.create([ dateObject.year, dateObject.month, 1 ])
};


/**
 * Validate a date as enabled and shift if needed.
 */
DatePicker.prototype.validate = function( type, dateObject, options ) {

    var calendar = this,

        // Keep a reference to the original date.
        originalDateObject = dateObject,

        // Make sure we have an interval.
        interval = options && options.interval ? options.interval : 1,

        // Check if the calendar enabled dates are inverted.
        isFlippedBase = calendar.item.enable === -1,

        // Check if we have any enabled dates after/before now.
        hasEnabledBeforeTarget, hasEnabledAfterTarget,

        // The min & max limits.
        minLimitObject = calendar.item.min,
        maxLimitObject = calendar.item.max,

        // Check if we’ve reached the limit during shifting.
        reachedMin, reachedMax,

        // Check if the calendar is inverted and at least one weekday is enabled.
        hasEnabledWeekdays = isFlippedBase && calendar.item.disable.filter( function( value ) {

            // If there’s a date, check where it is relative to the target.
            if ( $$$1.isArray( value ) ) {
                var dateTime = calendar.create( value ).pick;
                if ( dateTime < dateObject.pick ) hasEnabledBeforeTarget = true;
                else if ( dateTime > dateObject.pick ) hasEnabledAfterTarget = true;
            }

            // Return only integers for enabled weekdays.
            return _.isInteger( value )
        }).length;/*,

        safety = 100*/



    // Cases to validate for:
    // [1] Not inverted and date disabled.
    // [2] Inverted and some dates enabled.
    // [3] Not inverted and out of range.
    //
    // Cases to **not** validate for:
    // • Navigating months.
    // • Not inverted and date enabled.
    // • Inverted and all dates disabled.
    // • ..and anything else.
    if ( !options || (!options.nav && !options.defaultValue) ) if (
        /* 1 */ ( !isFlippedBase && calendar.disabled( dateObject ) ) ||
        /* 2 */ ( isFlippedBase && calendar.disabled( dateObject ) && ( hasEnabledWeekdays || hasEnabledBeforeTarget || hasEnabledAfterTarget ) ) ||
        /* 3 */ ( !isFlippedBase && (dateObject.pick <= minLimitObject.pick || dateObject.pick >= maxLimitObject.pick) )
    ) {


        // When inverted, flip the direction if there aren’t any enabled weekdays
        // and there are no enabled dates in the direction of the interval.
        if ( isFlippedBase && !hasEnabledWeekdays && ( ( !hasEnabledAfterTarget && interval > 0 ) || ( !hasEnabledBeforeTarget && interval < 0 ) ) ) {
            interval *= -1;
        }


        // Keep looping until we reach an enabled date.
        while ( /*safety &&*/ calendar.disabled( dateObject ) ) {

            /*safety -= 1
            if ( !safety ) {
                throw 'Fell into an infinite loop while validating ' + dateObject.obj + '.'
            }*/


            // If we’ve looped into the next/prev month with a large interval, return to the original date and flatten the interval.
            if ( Math.abs( interval ) > 1 && ( dateObject.month < originalDateObject.month || dateObject.month > originalDateObject.month ) ) {
                dateObject = originalDateObject;
                interval = interval > 0 ? 1 : -1;
            }


            // If we’ve reached the min/max limit, reverse the direction, flatten the interval and set it to the limit.
            if ( dateObject.pick <= minLimitObject.pick ) {
                reachedMin = true;
                interval = 1;
                dateObject = calendar.create([
                    minLimitObject.year,
                    minLimitObject.month,
                    minLimitObject.date + (dateObject.pick === minLimitObject.pick ? 0 : -1)
                ]);
            }
            else if ( dateObject.pick >= maxLimitObject.pick ) {
                reachedMax = true;
                interval = -1;
                dateObject = calendar.create([
                    maxLimitObject.year,
                    maxLimitObject.month,
                    maxLimitObject.date + (dateObject.pick === maxLimitObject.pick ? 0 : 1)
                ]);
            }


            // If we’ve reached both limits, just break out of the loop.
            if ( reachedMin && reachedMax ) {
                break
            }


            // Finally, create the shifted date using the interval and keep looping.
            dateObject = calendar.create([ dateObject.year, dateObject.month, dateObject.date + interval ]);
        }

    } //endif


    // Return the date object settled on.
    return dateObject
}; //DatePicker.prototype.validate


/**
 * Check if a date is disabled.
 */
DatePicker.prototype.disabled = function( dateToVerify ) {

    var
        calendar = this,

        // Filter through the disabled dates to check if this is one.
        isDisabledMatch = calendar.item.disable.filter( function( dateToDisable ) {

            // If the date is a number, match the weekday with 0index and `firstDay` check.
            if ( _.isInteger( dateToDisable ) ) {
                return dateToVerify.day === ( calendar.settings.firstDay ? dateToDisable : dateToDisable - 1 ) % 7
            }

            // If it’s an array or a native JS date, create and match the exact date.
            if ( $$$1.isArray( dateToDisable ) || _.isDate( dateToDisable ) ) {
                return dateToVerify.pick === calendar.create( dateToDisable ).pick
            }

            // If it’s an object, match a date within the “from” and “to” range.
            if ( $$$1.isPlainObject( dateToDisable ) ) {
                return calendar.withinRange( dateToDisable, dateToVerify )
            }
        });

    // If this date matches a disabled date, confirm it’s not inverted.
    isDisabledMatch = isDisabledMatch.length && !isDisabledMatch.filter(function( dateToDisable ) {
        return $$$1.isArray( dateToDisable ) && dateToDisable[3] == 'inverted' ||
            $$$1.isPlainObject( dateToDisable ) && dateToDisable.inverted
    }).length;

    // Check the calendar “enabled” flag and respectively flip the
    // disabled state. Then also check if it’s beyond the min/max limits.
    return calendar.item.enable === -1 ? !isDisabledMatch : isDisabledMatch ||
        dateToVerify.pick < calendar.item.min.pick ||
        dateToVerify.pick > calendar.item.max.pick

}; //DatePicker.prototype.disabled


/**
 * Parse a string into a usable type.
 */
DatePicker.prototype.parse = function( type, value, options ) {

    var calendar = this,
        parsingObject = {};

    // If it’s already parsed, we’re good.
    if ( !value || typeof value != 'string' ) {
        return value
    }

    // We need a `.format` to parse the value with.
    if ( !( options && options.format ) ) {
        options = options || {};
        options.format = calendar.settings.format;
    }

    // Convert the format into an array and then map through it.
    calendar.formats.toArray( options.format ).map( function( label ) {

        var
            // Grab the formatting label.
            formattingLabel = calendar.formats[ label ],

            // The format length is from the formatting label function or the
            // label length without the escaping exclamation (!) mark.
            formatLength = formattingLabel ? _.trigger( formattingLabel, calendar, [ value, parsingObject ] ) : label.replace( /^!/, '' ).length;

        // If there's a format label, split the value up to the format length.
        // Then add it to the parsing object with appropriate label.
        if ( formattingLabel ) {
            parsingObject[ label ] = value.substr( 0, formatLength );
        }

        // Update the value as the substring from format length to end.
        value = value.substr( formatLength );
    });

    // Compensate for month 0index.
    return [
        parsingObject.yyyy || parsingObject.yy,
        +( parsingObject.mm || parsingObject.m ) - 1,
        parsingObject.dd || parsingObject.d
    ]
}; //DatePicker.prototype.parse


/**
 * Various formats to display the object in.
 */
DatePicker.prototype.formats = (function() {

    // Return the length of the first word in a collection.
    function getWordLengthFromCollection( string, collection, dateObject ) {

        // Grab the first word from the string.
        // Regex pattern from http://stackoverflow.com/q/150033
        var word = string.match( /[^\x00-\x7F]+|\w+/ )[ 0 ];

        // If there's no month index, add it to the date object
        if ( !dateObject.mm && !dateObject.m ) {
            dateObject.m = collection.indexOf( word ) + 1;
        }

        // Return the length of the word.
        return word.length
    }

    // Get the length of the first word in a string.
    function getFirstWordLength( string ) {
        return string.match( /\w+/ )[ 0 ].length
    }

    return {

        d: function( string, dateObject ) {

            // If there's string, then get the digits length.
            // Otherwise return the selected date.
            return string ? _.digits( string ) : dateObject.date
        },
        dd: function( string, dateObject ) {

            // If there's a string, then the length is always 2.
            // Otherwise return the selected date with a leading zero.
            return string ? 2 : _.lead( dateObject.date )
        },
        ddd: function( string, dateObject ) {

            // If there's a string, then get the length of the first word.
            // Otherwise return the short selected weekday.
            return string ? getFirstWordLength( string ) : this.settings.weekdaysShort[ dateObject.day ]
        },
        dddd: function( string, dateObject ) {

            // If there's a string, then get the length of the first word.
            // Otherwise return the full selected weekday.
            return string ? getFirstWordLength( string ) : this.settings.weekdaysFull[ dateObject.day ]
        },
        m: function( string, dateObject ) {

            // If there's a string, then get the length of the digits
            // Otherwise return the selected month with 0index compensation.
            return string ? _.digits( string ) : dateObject.month + 1
        },
        mm: function( string, dateObject ) {

            // If there's a string, then the length is always 2.
            // Otherwise return the selected month with 0index and leading zero.
            return string ? 2 : _.lead( dateObject.month + 1 )
        },
        mmm: function( string, dateObject ) {

            var collection = this.settings.monthsShort;

            // If there's a string, get length of the relevant month from the short
            // months collection. Otherwise return the selected month from that collection.
            return string ? getWordLengthFromCollection( string, collection, dateObject ) : collection[ dateObject.month ]
        },
        mmmm: function( string, dateObject ) {

            var collection = this.settings.monthsFull;

            // If there's a string, get length of the relevant month from the full
            // months collection. Otherwise return the selected month from that collection.
            return string ? getWordLengthFromCollection( string, collection, dateObject ) : collection[ dateObject.month ]
        },
        yy: function( string, dateObject ) {

            // If there's a string, then the length is always 2.
            // Otherwise return the selected year by slicing out the first 2 digits.
            return string ? 2 : ( '' + dateObject.year ).slice( 2 )
        },
        yyyy: function( string, dateObject ) {

            // If there's a string, then the length is always 4.
            // Otherwise return the selected year.
            return string ? 4 : dateObject.year
        },

        // Create an array by splitting the formatting string passed.
        toArray: function( formatString ) { return formatString.split( /(d{1,4}|m{1,4}|y{4}|yy|!.)/g ) },

        // Format an object into a string using the formatting options.
        toString: function ( formatString, itemObject ) {
            var calendar = this;
            return calendar.formats.toArray( formatString ).map( function( label ) {
                return _.trigger( calendar.formats[ label ], calendar, [ 0, itemObject ] ) || label.replace( /^!/, '' )
            }).join( '' )
        }
    }
})(); //DatePicker.prototype.formats




/**
 * Check if two date units are the exact.
 */
DatePicker.prototype.isDateExact = function( one, two ) {

    var calendar = this;

    // When we’re working with weekdays, do a direct comparison.
    if (
        ( _.isInteger( one ) && _.isInteger( two ) ) ||
        ( typeof one == 'boolean' && typeof two == 'boolean' )
     ) {
        return one === two
    }

    // When we’re working with date representations, compare the “pick” value.
    if (
        ( _.isDate( one ) || $$$1.isArray( one ) ) &&
        ( _.isDate( two ) || $$$1.isArray( two ) )
    ) {
        return calendar.create( one ).pick === calendar.create( two ).pick
    }

    // When we’re working with range objects, compare the “from” and “to”.
    if ( $$$1.isPlainObject( one ) && $$$1.isPlainObject( two ) ) {
        return calendar.isDateExact( one.from, two.from ) && calendar.isDateExact( one.to, two.to )
    }

    return false
};


/**
 * Check if two date units overlap.
 */
DatePicker.prototype.isDateOverlap = function( one, two ) {

    var calendar = this,
        firstDay = calendar.settings.firstDay ? 1 : 0;

    // When we’re working with a weekday index, compare the days.
    if ( _.isInteger( one ) && ( _.isDate( two ) || $$$1.isArray( two ) ) ) {
        one = one % 7 + firstDay;
        return one === calendar.create( two ).day + 1
    }
    if ( _.isInteger( two ) && ( _.isDate( one ) || $$$1.isArray( one ) ) ) {
        two = two % 7 + firstDay;
        return two === calendar.create( one ).day + 1
    }

    // When we’re working with range objects, check if the ranges overlap.
    if ( $$$1.isPlainObject( one ) && $$$1.isPlainObject( two ) ) {
        return calendar.overlapRanges( one, two )
    }

    return false
};


/**
 * Flip the “enabled” state.
 */
DatePicker.prototype.flipEnable = function(val) {
    var itemObject = this.item;
    itemObject.enable = val || (itemObject.enable == -1 ? 1 : -1);
};


/**
 * Mark a collection of dates as “disabled”.
 */
DatePicker.prototype.deactivate = function( type, datesToDisable ) {

    var calendar = this,
        disabledItems = calendar.item.disable.slice(0);


    // If we’re flipping, that’s all we need to do.
    if ( datesToDisable == 'flip' ) {
        calendar.flipEnable();
    }

    else if ( datesToDisable === false ) {
        calendar.flipEnable(1);
        disabledItems = [];
    }

    else if ( datesToDisable === true ) {
        calendar.flipEnable(-1);
        disabledItems = [];
    }

    // Otherwise go through the dates to disable.
    else {

        datesToDisable.map(function( unitToDisable ) {

            var matchFound;

            // When we have disabled items, check for matches.
            // If something is matched, immediately break out.
            for ( var index = 0; index < disabledItems.length; index += 1 ) {
                if ( calendar.isDateExact( unitToDisable, disabledItems[index] ) ) {
                    matchFound = true;
                    break
                }
            }

            // If nothing was found, add the validated unit to the collection.
            if ( !matchFound ) {
                if (
                    _.isInteger( unitToDisable ) ||
                    _.isDate( unitToDisable ) ||
                    $$$1.isArray( unitToDisable ) ||
                    ( $$$1.isPlainObject( unitToDisable ) && unitToDisable.from && unitToDisable.to )
                ) {
                    disabledItems.push( unitToDisable );
                }
            }
        });
    }

    // Return the updated collection.
    return disabledItems
}; //DatePicker.prototype.deactivate


/**
 * Mark a collection of dates as “enabled”.
 */
DatePicker.prototype.activate = function( type, datesToEnable ) {

    var calendar = this,
        disabledItems = calendar.item.disable,
        disabledItemsCount = disabledItems.length;

    // If we’re flipping, that’s all we need to do.
    if ( datesToEnable == 'flip' ) {
        calendar.flipEnable();
    }

    else if ( datesToEnable === true ) {
        calendar.flipEnable(1);
        disabledItems = [];
    }

    else if ( datesToEnable === false ) {
        calendar.flipEnable(-1);
        disabledItems = [];
    }

    // Otherwise go through the disabled dates.
    else {

        datesToEnable.map(function( unitToEnable ) {

            var matchFound,
                disabledUnit,
                index,
                isExactRange;

            // Go through the disabled items and try to find a match.
            for ( index = 0; index < disabledItemsCount; index += 1 ) {

                disabledUnit = disabledItems[index];

                // When an exact match is found, remove it from the collection.
                if ( calendar.isDateExact( disabledUnit, unitToEnable ) ) {
                    matchFound = disabledItems[index] = null;
                    isExactRange = true;
                    break
                }

                // When an overlapped match is found, add the “inverted” state to it.
                else if ( calendar.isDateOverlap( disabledUnit, unitToEnable ) ) {
                    if ( $$$1.isPlainObject( unitToEnable ) ) {
                        unitToEnable.inverted = true;
                        matchFound = unitToEnable;
                    }
                    else if ( $$$1.isArray( unitToEnable ) ) {
                        matchFound = unitToEnable;
                        if ( !matchFound[3] ) matchFound.push( 'inverted' );
                    }
                    else if ( _.isDate( unitToEnable ) ) {
                        matchFound = [ unitToEnable.getFullYear(), unitToEnable.getMonth(), unitToEnable.getDate(), 'inverted' ];
                    }
                    break
                }
            }

            // If a match was found, remove a previous duplicate entry.
            if ( matchFound ) for ( index = 0; index < disabledItemsCount; index += 1 ) {
                if ( calendar.isDateExact( disabledItems[index], unitToEnable ) ) {
                    disabledItems[index] = null;
                    break
                }
            }

            // In the event that we’re dealing with an exact range of dates,
            // make sure there are no “inverted” dates because of it.
            if ( isExactRange ) for ( index = 0; index < disabledItemsCount; index += 1 ) {
                if ( calendar.isDateOverlap( disabledItems[index], unitToEnable ) ) {
                    disabledItems[index] = null;
                    break
                }
            }

            // If something is still matched, add it into the collection.
            if ( matchFound ) {
                disabledItems.push( matchFound );
            }
        });
    }

    // Return the updated collection.
    return disabledItems.filter(function( val ) { return val != null })
}; //DatePicker.prototype.activate


/**
 * Create a string for the nodes in the picker.
 */
DatePicker.prototype.nodes = function( isOpen ) {

    var
        calendar = this,
        settings = calendar.settings,
        calendarItem = calendar.item,
        nowObject = calendarItem.now,
        selectedObject = calendarItem.select,
        highlightedObject = calendarItem.highlight,
        viewsetObject = calendarItem.view,
        disabledCollection = calendarItem.disable,
        minLimitObject = calendarItem.min,
        maxLimitObject = calendarItem.max,


        // Create the calendar table head using a copy of weekday labels collection.
        // * We do a copy so we don't mutate the original array.
        tableHead = (function( collection, fullCollection ) {

            // If the first day should be Monday, move Sunday to the end.
            if ( settings.firstDay ) {
                collection.push( collection.shift() );
                fullCollection.push( fullCollection.shift() );
            }

            // Create and return the table head group.
            return _.node(
                'thead',
                _.node(
                    'tr',
                    _.group({
                        min: 0,
                        max: DAYS_IN_WEEK - 1,
                        i: 1,
                        node: 'th',
                        item: function( counter ) {
                            return [
                                collection[ counter ],
                                settings.klass.weekdays,
                                'scope=col title="' + fullCollection[ counter ] + '"'
                            ]
                        }
                    })
                )
            ) //endreturn
        })( ( settings.showWeekdaysFull ? settings.weekdaysFull : settings.weekdaysShort ).slice( 0 ), settings.weekdaysFull.slice( 0 ) ), //tableHead


        // Create the nav for next/prev month.
        createMonthNav = function( next ) {

            // Otherwise, return the created month tag.
            return _.node(
                'div',
                ' ',
                settings.klass[ 'nav' + ( next ? 'Next' : 'Prev' ) ] + (

                    // If the focused month is outside the range, disabled the button.
                    ( next && viewsetObject.year >= maxLimitObject.year && viewsetObject.month >= maxLimitObject.month ) ||
                    ( !next && viewsetObject.year <= minLimitObject.year && viewsetObject.month <= minLimitObject.month ) ?
                    ' ' + settings.klass.navDisabled : ''
                ),
                'data-nav=' + ( next || -1 ) + ' ' +
                _.ariaAttr({
                    role: 'button',
                    controls: calendar.$node[0].id + '_table'
                }) + ' ' +
                'title="' + (next ? settings.labelMonthNext : settings.labelMonthPrev ) + '"'
            ) //endreturn
        }, //createMonthNav


        // Create the month label.
        createMonthLabel = function() {

            var monthsCollection = settings.showMonthsShort ? settings.monthsShort : settings.monthsFull;

            // If there are months to select, add a dropdown menu.
            if ( settings.selectMonths ) {

                return _.node( 'select',
                    _.group({
                        min: 0,
                        max: 11,
                        i: 1,
                        node: 'option',
                        item: function( loopedMonth ) {

                            return [

                                // The looped month and no classes.
                                monthsCollection[ loopedMonth ], 0,

                                // Set the value and selected index.
                                'value=' + loopedMonth +
                                ( viewsetObject.month == loopedMonth ? ' selected' : '' ) +
                                (
                                    (
                                        ( viewsetObject.year == minLimitObject.year && loopedMonth < minLimitObject.month ) ||
                                        ( viewsetObject.year == maxLimitObject.year && loopedMonth > maxLimitObject.month )
                                    ) ?
                                    ' disabled' : ''
                                )
                            ]
                        }
                    }),
                    settings.klass.selectMonth,
                    ( isOpen ? '' : 'disabled' ) + ' ' +
                    _.ariaAttr({ controls: calendar.$node[0].id + '_table' }) + ' ' +
                    'title="' + settings.labelMonthSelect + '"'
                )
            }

            // If there's a need for a month selector
            return _.node( 'div', monthsCollection[ viewsetObject.month ], settings.klass.month )
        }, //createMonthLabel


        // Create the year label.
        createYearLabel = function() {

            var focusedYear = viewsetObject.year,

            // If years selector is set to a literal "true", set it to 5. Otherwise
            // divide in half to get half before and half after focused year.
            numberYears = settings.selectYears === true ? 5 : ~~( settings.selectYears / 2 );

            // If there are years to select, add a dropdown menu.
            if ( numberYears ) {

                var
                    minYear = minLimitObject.year,
                    maxYear = maxLimitObject.year,
                    lowestYear = focusedYear - numberYears,
                    highestYear = focusedYear + numberYears;

                // If the min year is greater than the lowest year, increase the highest year
                // by the difference and set the lowest year to the min year.
                if ( minYear > lowestYear ) {
                    highestYear += minYear - lowestYear;
                    lowestYear = minYear;
                }

                // If the max year is less than the highest year, decrease the lowest year
                // by the lower of the two: available and needed years. Then set the
                // highest year to the max year.
                if ( maxYear < highestYear ) {

                    var availableYears = lowestYear - minYear,
                        neededYears = highestYear - maxYear;

                    lowestYear -= availableYears > neededYears ? neededYears : availableYears;
                    highestYear = maxYear;
                }

                return _.node( 'select',
                    _.group({
                        min: lowestYear,
                        max: highestYear,
                        i: 1,
                        node: 'option',
                        item: function( loopedYear ) {
                            return [

                                // The looped year and no classes.
                                loopedYear, 0,

                                // Set the value and selected index.
                                'value=' + loopedYear + ( focusedYear == loopedYear ? ' selected' : '' )
                            ]
                        }
                    }),
                    settings.klass.selectYear,
                    ( isOpen ? '' : 'disabled' ) + ' ' + _.ariaAttr({ controls: calendar.$node[0].id + '_table' }) + ' ' +
                    'title="' + settings.labelYearSelect + '"'
                )
            }

            // Otherwise just return the year focused
            return _.node( 'div', focusedYear, settings.klass.year )
        }; //createYearLabel


    // Create and return the entire calendar.
    return _.node(
        'div',
        ( settings.selectYears ? createYearLabel() + createMonthLabel() : createMonthLabel() + createYearLabel() ) +
        createMonthNav() + createMonthNav( 1 ),
        settings.klass.header
    ) + _.node(
        'table',
        tableHead +
        _.node(
            'tbody',
            _.group({
                min: 0,
                max: WEEKS_IN_CALENDAR - 1,
                i: 1,
                node: 'tr',
                item: function( rowCounter ) {

                    // If Monday is the first day and the month starts on Sunday, shift the date back a week.
                    var shiftDateBy = settings.firstDay && calendar.create([ viewsetObject.year, viewsetObject.month, 1 ]).day === 0 ? -7 : 0;

                    return [
                        _.group({
                            min: DAYS_IN_WEEK * rowCounter - viewsetObject.day + shiftDateBy + 1, // Add 1 for weekday 0index
                            max: function() {
                                return this.min + DAYS_IN_WEEK - 1
                            },
                            i: 1,
                            node: 'td',
                            item: function( targetDate ) {

                                // Convert the time date from a relative date to a target date.
                                targetDate = calendar.create([ viewsetObject.year, viewsetObject.month, targetDate + ( settings.firstDay ? 1 : 0 ) ]);

                                var isSelected = selectedObject && selectedObject.pick == targetDate.pick,
                                    isHighlighted = highlightedObject && highlightedObject.pick == targetDate.pick,
                                    isDisabled = disabledCollection && calendar.disabled( targetDate ) || targetDate.pick < minLimitObject.pick || targetDate.pick > maxLimitObject.pick,
                                    formattedDate = _.trigger( calendar.formats.toString, calendar, [ settings.format, targetDate ] );

                                return [
                                    _.node(
                                        'div',
                                        targetDate.date,
                                        (function( klasses ) {

                                            // Add the `infocus` or `outfocus` classes based on month in view.
                                            klasses.push( viewsetObject.month == targetDate.month ? settings.klass.infocus : settings.klass.outfocus );

                                            // Add the `today` class if needed.
                                            if ( nowObject.pick == targetDate.pick ) {
                                                klasses.push( settings.klass.now );
                                            }

                                            // Add the `selected` class if something's selected and the time matches.
                                            if ( isSelected ) {
                                                klasses.push( settings.klass.selected );
                                            }

                                            // Add the `highlighted` class if something's highlighted and the time matches.
                                            if ( isHighlighted ) {
                                                klasses.push( settings.klass.highlighted );
                                            }

                                            // Add the `disabled` class if something's disabled and the object matches.
                                            if ( isDisabled ) {
                                                klasses.push( settings.klass.disabled );
                                            }

                                            return klasses.join( ' ' )
                                        })([ settings.klass.day ]),
                                        'data-pick=' + targetDate.pick + ' ' + _.ariaAttr({
                                            role: 'gridcell',
                                            label: formattedDate,
                                            selected: isSelected && calendar.$node.val() === formattedDate ? true : null,
                                            activedescendant: isHighlighted ? true : null,
                                            disabled: isDisabled ? true : null
                                        })
                                    ),
                                    '',
                                    _.ariaAttr({ role: 'presentation' })
                                ] //endreturn
                            }
                        })
                    ] //endreturn
                }
            })
        ),
        settings.klass.table,
        'id="' + calendar.$node[0].id + '_table' + '" ' + _.ariaAttr({
            role: 'grid',
            controls: calendar.$node[0].id,
            readonly: true
        })
    ) +

    // * For Firefox forms to submit, make sure to set the buttons’ `type` attributes as “button”.
    _.node(
        'div',
        _.node( 'button', settings.today, settings.klass.buttonToday,
            'type=button data-pick=' + nowObject.pick +
            ( isOpen && !calendar.disabled(nowObject) ? '' : ' disabled' ) + ' ' +
            _.ariaAttr({ controls: calendar.$node[0].id }) ) +
        _.node( 'button', settings.clear, settings.klass.buttonClear,
            'type=button data-clear=1' +
            ( isOpen ? '' : ' disabled' ) + ' ' +
            _.ariaAttr({ controls: calendar.$node[0].id }) ) +
        _.node('button', settings.close, settings.klass.buttonClose,
            'type=button data-close=true ' +
            ( isOpen ? '' : ' disabled' ) + ' ' +
            _.ariaAttr({ controls: calendar.$node[0].id }) ),
        settings.klass.footer
    ) //endreturn
}; //DatePicker.prototype.nodes




/**
 * The date picker defaults.
 */
DatePicker.defaults = (function( prefix ) {

    return {

        // The title label to use for the month nav buttons
        labelMonthNext: 'Next month',
        labelMonthPrev: 'Previous month',

        // The title label to use for the dropdown selectors
        labelMonthSelect: 'Select a month',
        labelYearSelect: 'Select a year',

        // Months and weekdays
        monthsFull: [ 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December' ],
        monthsShort: [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec' ],
        weekdaysFull: [ 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday' ],
        weekdaysShort: [ 'Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat' ],

        // Today and clear
        today: 'Today',
        clear: 'Clear',
        close: 'Close',

        // Picker close behavior
        closeOnSelect: true,
        closeOnClear: true,

        // The format to show on the `input` element
        format: 'd mmmm, yyyy',

        // Classes
        klass: {

            table: prefix + 'table',

            header: prefix + 'header',

            navPrev: prefix + 'nav--prev',
            navNext: prefix + 'nav--next',
            navDisabled: prefix + 'nav--disabled',

            month: prefix + 'month',
            year: prefix + 'year',

            selectMonth: prefix + 'select--month',
            selectYear: prefix + 'select--year',

            weekdays: prefix + 'weekday',

            day: prefix + 'day',
            disabled: prefix + 'day--disabled',
            selected: prefix + 'day--selected',
            highlighted: prefix + 'day--highlighted',
            now: prefix + 'day--today',
            infocus: prefix + 'day--infocus',
            outfocus: prefix + 'day--outfocus',

            footer: prefix + 'footer',

            buttonClear: prefix + 'button--clear',
            buttonToday: prefix + 'button--today',
            buttonClose: prefix + 'button--close'
        }
    }
})( Picker.klasses().picker + '__' );





/**
 * Extend the picker to add the date picker.
 */
Picker.extend( 'pickadate', DatePicker );


}));
});

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _extends() {
  _extends = Object.assign || function (target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i];

      for (var key in source) {
        if (Object.prototype.hasOwnProperty.call(source, key)) {
          target[key] = source[key];
        }
      }
    }

    return target;
  };

  return _extends.apply(this, arguments);
}

/*
 * Customise pickadate JavaScript for Material
 */
(function ($$$1) {
  var Datepicker = function Datepicker(element, options) {
    this._element = element;
    this._options = options;
  };

  Datepicker.DEFAULTS = {
    cancel: 'Cancel',
    closeOnCancel: true,
    closeOnSelect: false,
    container: 'body',
    disable: [],
    firstDay: 0,
    format: 'd/m/yyyy',
    formatSubmit: '',
    klass: {
      // button
      buttonClear: 'btn btn-outline-primary picker-button-clear',
      buttonClose: 'btn btn-outline-primary picker-button-close',
      buttonToday: 'btn btn-outline-primary picker-button-today',
      // day
      day: 'picker-day',
      disabled: 'picker-day-disabled',
      highlighted: 'picker-day-highlighted',
      infocus: 'picker-day-infocus',
      now: 'picker-day-today',
      outfocus: 'picker-day-outfocus',
      selected: 'picker-day-selected',
      weekdays: 'picker-weekday',
      // element
      box: 'picker-box',
      footer: 'picker-footer',
      frame: 'picker-frame',
      header: 'picker-header',
      holder: 'picker-holder',
      table: 'picker-table',
      wrap: 'picker-wrap',
      // input element
      active: 'picker-input-active',
      input: 'picker-input',
      // month and year nav
      month: 'picker-month',
      navDisabled: 'picker-nav-disabled',
      navNext: 'material-icons picker-nav-next',
      navPrev: 'material-icons picker-nav-prev',
      selectMonth: 'picker-select-month',
      selectYear: 'picker-select-year',
      year: 'picker-year',
      // root picker
      focused: 'picker-focused',
      opened: 'picker-opened',
      picker: 'picker'
    },
    max: false,
    min: false,
    monthsFull: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    ok: 'OK',
    onClose: false,
    onOpen: false,
    onRender: false,
    onSet: false,
    onStart: false,
    onStop: false,
    selectMonths: false,
    selectYears: false,
    today: '',
    weekdaysFull: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
    weekdaysShort: ['S', 'M', 'T', 'W', 'T', 'F', 'S']
  };

  Datepicker.prototype.display = function (datepickerApi, datepickerRoot, datepickerValue) {
    $$$1('.picker-date-display', datepickerRoot).remove();
    $$$1('.picker-wrap', datepickerRoot).prepend('<div class="picker-date-display">' + '<div class="picker-date-display-top">' + '<span class="picker-year-display">' + datepickerApi.get(datepickerValue, 'yyyy') + '</span>' + '</div>' + '<div class="picker-date-display-bottom">' + '<span class="picker-weekday-display">' + datepickerApi.get(datepickerValue, 'dddd') + '</span>' + '<span class="picker-day-display">' + datepickerApi.get(datepickerValue, 'd') + '</span>' + '<span class="picker-month-display">' + datepickerApi.get(datepickerValue, 'mmm') + '</span>' + '</div>' + '</div>');
  };

  Datepicker.prototype.show = function () {
    var that = this;
    $$$1(this._element).pickadate({
      clear: that._options.cancel,
      close: that._options.ok,
      closeOnClear: that._options.closeOnCancel,
      closeOnSelect: that._options.closeOnSelect,
      container: that._options.container,
      disable: that._options.disable,
      firstDay: that._options.firstDay,
      format: that._options.format,
      formatSubmit: that._options.formatSubmit,
      klass: that._options.klass,
      max: that._options.max,
      min: that._options.min,
      monthsFull: that._options.monthsFull,
      monthsShort: that._options.monthsShort,
      onClose: that._options.onClose,
      onOpen: that._options.onOpen,
      onRender: that._options.onRender,
      onSet: that._options.onSet,
      onStart: that._options.onStart,
      onStop: that._options.onStop,
      selectMonths: that._options.selectMonths,
      selectYears: that._options.selectYears,
      today: that._options.today,
      weekdaysFull: that._options.weekdaysFull,
      weekdaysShort: that._options.weekdaysShort
    });
    var datepickerApi = $$$1(this._element).pickadate('picker'),
        datepickerNode = datepickerApi.$node,
        datepickerRoot = datepickerApi.$root;
    datepickerApi.on({
      close: function close() {
        $$$1(document.activeElement).blur();
      },
      open: function open() {
        if (!$$$1('.picker__date-display', datepickerRoot).length) {
          that.display(datepickerApi, datepickerRoot, 'highlight');
        }
      },
      set: function set() {
        if (datepickerApi.get('select') !== null) {
          that.display(datepickerApi, datepickerRoot, 'select');
        }
      }
    });
  };

  function Plugin(option) {
    return this.each(function () {
      var data = $$$1(this).data('bs.pickdate');
      var options = $$$1.extend({}, Datepicker.DEFAULTS, $$$1(this).data(), typeof option == 'object' && option);

      if (!data) {
        $$$1(this).data('bs.pickdate', data = new Datepicker(this, options));
      }

      data.show();
    });
  }

  var old = $$$1.fn.pickdate;
  $$$1.fn.pickdate = Plugin;
  $$$1.fn.pickdate.Constructor = Datepicker;

  $$$1.fn.pickdate.noConflict = function () {
    $$$1.fn.pickdate = old;
    return this;
  };
})(jQuery);

/*
 * expansion panel
 * upon selecting the collapsed panel,
 * it expands to display in full
 */

var ExpansionPanel = function ($$$1) {
  // constants >>>
  var DATA_KEY = 'bs.collapse';
  var EVENT_KEY = "." + DATA_KEY;
  var ClassName = {
    SHOW: 'show',
    SHOW_PREDECESSOR: 'show-predecessor'
  };
  var Event = {
    HIDE: "hide" + EVENT_KEY,
    SHOW: "show" + EVENT_KEY
  };
  var Selector = {
    PANEL: '.expansion-panel',
    PANEL_BODY: '.expansion-panel .collapse' // <<< constants

  };
  $$$1(document).on("" + Event.HIDE, Selector.PANEL_BODY, function () {
    var target = $$$1(this).closest(Selector.PANEL);
    target.removeClass(ClassName.SHOW);
    var predecessor = target.prev(Selector.PANEL);

    if (predecessor.length) {
      predecessor.removeClass(ClassName.SHOW_PREDECESSOR);
    }
  }).on("" + Event.SHOW, Selector.PANEL_BODY, function () {
    var target = $$$1(this).closest(Selector.PANEL);
    target.addClass(ClassName.SHOW);
    var predecessor = target.prev(Selector.PANEL);

    if (predecessor.length) {
      predecessor.addClass(ClassName.SHOW_PREDECESSOR);
    }
  });
}($);

/*
 * floating label
 * when a user engages with the text input field,
 * the floating inline labels move to float above the field
 */

var FloatingLabel = function ($$$1) {
  // constants >>>
  var DATA_KEY = 'md.floatinglabel';
  var EVENT_KEY = "." + DATA_KEY;
  var NAME = 'floatinglabel';
  var NO_CONFLICT = $$$1.fn[NAME];
  var ClassName = {
    IS_FOCUSED: 'is-focused',
    HAS_VALUE: 'has-value'
  };
  var Event = {
    CHANGE: "change" + EVENT_KEY,
    FOCUSIN: "focusin" + EVENT_KEY,
    FOCUSOUT: "focusout" + EVENT_KEY
  };
  var Selector = {
    DATA_PARENT: '.floating-label',
    DATA_TOGGLE: '.floating-label .form-control' // <<< constants

  };

  var FloatingLabel =
  /*#__PURE__*/
  function () {
    function FloatingLabel(element) {
      this._element = element;
      this._parent = $$$1(element).closest(Selector.DATA_PARENT)[0];
    }

    var _proto = FloatingLabel.prototype;

    _proto.change = function change() {
      if ($$$1(this._element).val() || $$$1(this._element).is('select') && $$$1('option:first-child', $$$1(this._element)).html().replace(' ', '') !== '') {
        $$$1(this._parent).addClass(ClassName.HAS_VALUE);
      } else {
        $$$1(this._parent).removeClass(ClassName.HAS_VALUE);
      }
    };

    _proto.focusin = function focusin() {
      $$$1(this._parent).addClass(ClassName.IS_FOCUSED);
    };

    _proto.focusout = function focusout() {
      $$$1(this._parent).removeClass(ClassName.IS_FOCUSED);
    };

    FloatingLabel._jQueryInterface = function _jQueryInterface(event) {
      return this.each(function () {
        var _event = event ? event : 'change';

        var data = $$$1(this).data(DATA_KEY);

        if (!data) {
          data = new FloatingLabel(this);
          $$$1(this).data(DATA_KEY, data);
        }

        if (typeof _event === 'string') {
          if (typeof data[_event] === 'undefined') {
            throw new Error("No method named \"" + _event + "\"");
          }

          data[_event]();
        }
      });
    };

    return FloatingLabel;
  }();

  $$$1(document).on(Event.CHANGE + " " + Event.FOCUSIN + " " + Event.FOCUSOUT, Selector.DATA_TOGGLE, function (event) {
    FloatingLabel._jQueryInterface.call($$$1(this), event.type);
  });
  $$$1.fn[NAME] = FloatingLabel._jQueryInterface;
  $$$1.fn[NAME].Constructor = FloatingLabel;

  $$$1.fn[NAME].noConflict = function () {
    $$$1.fn[NAME] = NO_CONFLICT;
    return FloatingLabel._jQueryInterface;
  };

  return FloatingLabel;
}($);

/*
 * Global util js
 * Based on Bootstrap's (v4.0.0) `util.js`
 */

var Util = function ($$$1) {
  var MAX_UID = 1000000;
  var transition = false;

  function getSpecialTransitionEndEvent() {
    return {
      bindType: transition.end,
      delegateType: transition.end,
      handle: function handle(event) {
        if ($$$1(event.target).is(this)) {
          return event.handleObj.handler.apply(this, arguments); // eslint-disable-line prefer-rest-params
        }

        return undefined; // eslint-disable-line no-undefined
      }
    };
  }

  function setTransitionEndSupport() {
    transition = transitionEndTest();
    $$$1.fn.emulateTransitionEnd = transitionEndEmulator;

    if (Util.supportsTransitionEnd()) {
      $$$1.event.special[Util.TRANSITION_END] = getSpecialTransitionEndEvent();
    }
  }

  function toType(obj) {
    return {}.toString.call(obj).match(/\s([a-z]+)/i)[1].toLowerCase();
  }

  function transitionEndEmulator(duration) {
    var _this = this;

    var called = false;
    $$$1(this).one(Util.TRANSITION_END, function () {
      called = true;
    });
    setTimeout(function () {
      if (!called) {
        Util.triggerTransitionEnd(_this);
      }
    }, duration);
    return this;
  }

  function transitionEndTest() {
    if (typeof window !== 'undefined' && window.QUnit) {
      return false;
    }

    return {
      end: 'transitionend'
    };
  }

  var Util = {
    TRANSITION_END: 'mdTransitionEnd',
    getSelectorFromElement: function getSelectorFromElement(element) {
      var selector = element.getAttribute('data-target');

      if (!selector || selector === '#') {
        selector = element.getAttribute('href') || '';
      }

      try {
        var $selector = $$$1(document).find(selector);
        return $selector.length > 0 ? selector : null;
      } catch (err) {
        return null;
      }
    },
    getUID: function getUID(prefix) {
      do {
        // eslint-disable-next-line no-bitwise
        prefix += ~~(Math.random() * MAX_UID);
      } while (document.getElementById(prefix));

      return prefix;
    },
    isElement: function isElement(obj) {
      return (obj[0] || obj).nodeType;
    },
    reflow: function reflow(element) {
      return element.offsetHeight;
    },
    supportsTransitionEnd: function supportsTransitionEnd() {
      return Boolean(transition);
    },
    triggerTransitionEnd: function triggerTransitionEnd(element) {
      $$$1(element).trigger(transition.end);
    },
    typeCheckConfig: function typeCheckConfig(componentName, config, configTypes) {
      for (var property in configTypes) {
        if (Object.prototype.hasOwnProperty.call(configTypes, property)) {
          var expectedTypes = configTypes[property];
          var value = config[property];
          var valueType = value && Util.isElement(value) ? 'element' : toType(value);

          if (!new RegExp(expectedTypes).test(valueType)) {
            throw new Error(componentName.toUpperCase() + ": " + ("Option \"" + property + "\" provided type \"" + valueType + "\" ") + ("but expected type \"" + expectedTypes + "\"."));
          }
        }
      }
    }
  };
  setTransitionEndSupport();
  return Util;
}($);

/*
 * Navigation drawer
 * Based on Bootstrap's (v4.0.0) `modal.js`
 */

var NavDrawer = function ($$$1) {
  // constants >>>
  var DATA_API_KEY = '.data-api';
  var DATA_KEY = 'md.navdrawer';
  var ESCAPE_KEYCODE = 27;
  var EVENT_KEY = "." + DATA_KEY;
  var NAME = 'navdrawer';
  var NO_CONFLICT = $$$1.fn[NAME];
  var Breakpoints = {
    DESKTOP: 992,
    TABLET: 576
  };
  var ClassName = {
    BACKDROP: 'navdrawer-backdrop',
    OPEN: 'navdrawer-open',
    SHOW: 'show'
  };
  var Default = {
    breakpoint: '',
    keyboard: true,
    show: true,
    type: 'default'
  };
  var DefaultType = {
    keyboard: 'boolean',
    show: 'boolean',
    type: 'string'
  };
  var Event = {
    CLICK_DATA_API: "click" + EVENT_KEY + DATA_API_KEY,
    CLICK_DISMISS: "click.dismiss" + EVENT_KEY,
    FOCUSIN: "focusin" + EVENT_KEY,
    HIDDEN: "hidden" + EVENT_KEY,
    HIDE: "hide" + EVENT_KEY,
    KEYDOWN_DISMISS: "keydown.dismiss" + EVENT_KEY,
    MOUSEDOWN_DISMISS: "mousedown.dismiss" + EVENT_KEY,
    MOUSEUP_DISMISS: "mouseup.dismiss" + EVENT_KEY,
    SHOW: "show" + EVENT_KEY,
    SHOWN: "shown" + EVENT_KEY
  };
  var Selector = {
    CONTENT: '.navdrawer-content',
    DATA_DISMISS: '[data-dismiss="navdrawer"]',
    DATA_TOGGLE: '[data-toggle="navdrawer"]'
  };
  var TransitionDurationEntering = {
    DESKTOP: 150,
    MOBILE: 225,
    TABLET: 292.5
  };
  var TransitionDurationLeaving = {
    DESKTOP: 130,
    MOBILE: 195,
    TABLET: 253.5 // <<< constants

  };

  var NavDrawer =
  /*#__PURE__*/
  function () {
    function NavDrawer(element, config) {
      this._backdrop = null;
      this._config = this._getConfig(config);
      this._content = $$$1(element).find(Selector.CONTENT)[0];
      this._element = element;
      this._ignoreBackdropClick = false;
      this._isShown = false;
      this._typeBreakpoint = this._config.breakpoint === '' ? '' : "-" + this._config.breakpoint;
    }

    var _proto = NavDrawer.prototype;

    _proto.hide = function hide(event) {
      var _this = this;

      if (event) {
        event.preventDefault();
      }

      if (this._isTransitioning || !this._isShown) {
        return;
      }

      var hideEvent = $$$1.Event(Event.HIDE);
      $$$1(this._element).trigger(hideEvent);

      if (!this._isShown || hideEvent.isDefaultPrevented()) {
        return;
      }

      this._isShown = false;
      var supportsTransition = Util.supportsTransitionEnd();

      if (supportsTransition) {
        this._isTransitioning = true;
      }

      this._setEscapeEvent();

      $$$1(document).off(Event.FOCUSIN);
      $$$1(this._element).removeClass(ClassName.SHOW);
      $$$1(this._element).off(Event.CLICK_DISMISS);
      $$$1(this._content).off(Event.MOUSEDOWN_DISMISS);

      if (supportsTransition) {
        $$$1(this._element).one(Util.TRANSITION_END, function (event) {
          return _this._hideNavdrawer(event);
        }).emulateTransitionEnd(this._getTransitionDuration(TransitionDurationLeaving));
      } else {
        this._hideNavdrawer();
      }

      this._showBackdrop();
    };

    _proto.show = function show(relatedTarget) {
      var _this2 = this;

      if (this._isTransitioning || this._isShown) {
        return;
      }

      if (Util.supportsTransitionEnd()) {
        this._isTransitioning = true;
      }

      var showEvent = $$$1.Event(Event.SHOW, {
        relatedTarget: relatedTarget
      });
      $$$1(this._element).trigger(showEvent);

      if (this._isShown || showEvent.isDefaultPrevented()) {
        return;
      }

      this._isShown = true;
      $$$1(document.body).addClass(ClassName.OPEN + "-" + this._config.type + this._typeBreakpoint);

      this._setEscapeEvent();

      $$$1(this._element).addClass(NAME + "-" + this._config.type + this._typeBreakpoint);
      $$$1(this._element).on(Event.CLICK_DISMISS, Selector.DATA_DISMISS, function (event) {
        return _this2.hide(event);
      });
      $$$1(this._content).on(Event.MOUSEDOWN_DISMISS, function () {
        $$$1(_this2._element).one(Event.MOUSEUP_DISMISS, function (event) {
          if ($$$1(event.target).is(_this2._element)) {
            _this2._ignoreBackdropClick = true;
          }
        });
      });

      this._showBackdrop();

      this._showElement(relatedTarget);
    };

    _proto.toggle = function toggle(relatedTarget) {
      return this._isShown ? this.hide() : this.show(relatedTarget);
    };

    _proto._enforceFocus = function _enforceFocus() {
      var _this3 = this;

      $$$1(document).off(Event.FOCUSIN).on(Event.FOCUSIN, function (event) {
        if (document !== event.target && _this3._element !== event.target && $$$1(_this3._element).has(event.target).length === 0) {
          _this3._element.focus();
        }
      });
    };

    _proto._getConfig = function _getConfig(config) {
      config = _extends({}, Default, config);
      Util.typeCheckConfig(NAME, config, DefaultType);
      return config;
    };

    _proto._getTransitionDuration = function _getTransitionDuration(duration) {
      if (window.innerWidth >= Breakpoints.DESKTOP) {
        return duration.DESKTOP;
      } else if (window.innerWidth >= Breakpoints.TABLET) {
        return duration.TABLET;
      }

      return duration.MOBILE;
    };

    _proto._hideNavdrawer = function _hideNavdrawer() {
      this._element.style.display = 'none';

      this._element.setAttribute('aria-hidden', true);

      this._isTransitioning = false;
      $$$1(document.body).removeClass(ClassName.OPEN + "-" + this._config.type + this._typeBreakpoint);
      $$$1(this._element).trigger(Event.HIDDEN);
    };

    _proto._removeBackdrop = function _removeBackdrop() {
      if (this._backdrop) {
        $$$1(this._backdrop).remove();
        this._backdrop = null;
      }
    };

    _proto._setEscapeEvent = function _setEscapeEvent() {
      var _this4 = this;

      if (this._isShown && this._config.keyboard) {
        $$$1(this._element).on(Event.KEYDOWN_DISMISS, function (event) {
          if (event.which === ESCAPE_KEYCODE) {
            event.preventDefault();

            _this4.hide();
          }
        });
      } else if (!this._isShown) {
        $$$1(this._element).off(Event.KEYDOWN_DISMISS);
      }
    };

    _proto._showBackdrop = function _showBackdrop() {
      var _this5 = this;

      var supportsTransition = Util.supportsTransitionEnd();

      if (this._isShown) {
        this._backdrop = document.createElement('div');
        $$$1(this._backdrop).addClass(ClassName.BACKDROP).addClass(ClassName.BACKDROP + "-" + this._config.type + this._typeBreakpoint).appendTo(document.body);
        $$$1(this._element).on(Event.CLICK_DISMISS, function (event) {
          if (_this5._ignoreBackdropClick) {
            _this5._ignoreBackdropClick = false;
            return;
          }

          if (event.target !== event.currentTarget) {
            return;
          }

          _this5.hide();
        });

        if (supportsTransition) {
          Util.reflow(this._backdrop);
        }

        $$$1(this._backdrop).addClass(ClassName.SHOW);
      } else if (!this._isShown && this._backdrop) {
        $$$1(this._backdrop).removeClass(ClassName.SHOW);

        this._removeBackdrop();
      }
    };

    _proto._showElement = function _showElement(relatedTarget) {
      var _this6 = this;

      var supportsTransition = Util.supportsTransitionEnd();

      if (!this._element.parentNode || this._element.parentNode.nodeType !== Node.ELEMENT_NODE) {
        document.body.appendChild(this._element);
      }

      this._element.style.display = 'block';

      this._element.removeAttribute('aria-hidden');

      if (supportsTransition) {
        Util.reflow(this._element);
      }

      $$$1(this._element).addClass(ClassName.SHOW);

      this._enforceFocus();

      var shownEvent = $$$1.Event(Event.SHOWN, {
        relatedTarget: relatedTarget
      });

      var transitionComplete = function transitionComplete() {
        _this6._element.focus();

        _this6._isTransitioning = false;
        $$$1(_this6._element).trigger(shownEvent);
      };

      if (supportsTransition) {
        $$$1(this._content).one(Util.TRANSITION_END, transitionComplete).emulateTransitionEnd(this._getTransitionDuration(TransitionDurationEntering));
      } else {
        transitionComplete();
      }
    };

    NavDrawer._jQueryInterface = function _jQueryInterface(config, relatedTarget) {
      return this.each(function () {
        var _config = _extends({}, NavDrawer.Default, $$$1(this).data(), typeof config === 'object' && config);

        var data = $$$1(this).data(DATA_KEY);

        if (!data) {
          data = new NavDrawer(this, _config);
          $$$1(this).data(DATA_KEY, data);
        }

        if (typeof config === 'string') {
          if (typeof data[config] === 'undefined') {
            throw new TypeError("No method named \"" + config + "\"");
          }

          data[config](relatedTarget);
        } else if (_config.show) {
          data.show(relatedTarget);
        }
      });
    };

    _createClass(NavDrawer, null, [{
      key: "Default",
      get: function get() {
        return Default;
      }
    }]);
    return NavDrawer;
  }();

  $$$1(document).on(Event.CLICK_DATA_API, Selector.DATA_TOGGLE, function (event) {
    var _this7 = this;

    var selector = Util.getSelectorFromElement(this);
    var target;

    if (selector) {
      target = $$$1(selector)[0];
    }

    var config = $$$1(target).data(DATA_KEY) ? 'toggle' : _extends({}, $$$1(target).data(), $$$1(this).data());

    if (this.tagName === 'A' || this.tagName === 'AREA') {
      event.preventDefault();
    }

    var $target = $$$1(target).one(Event.SHOW, function (showEvent) {
      if (showEvent.isDefaultPrevented()) {
        return;
      }

      $target.one(Event.HIDDEN, function () {
        if ($$$1(_this7).is(':visible')) {
          _this7.focus();
        }
      });
    });

    NavDrawer._jQueryInterface.call($$$1(target), config, this);
  });
  $$$1.fn[NAME] = NavDrawer._jQueryInterface;
  $$$1.fn[NAME].Constructor = NavDrawer;

  $$$1.fn[NAME].noConflict = function () {
    $$$1.fn[NAME] = NO_CONFLICT;
    return NavDrawer._jQueryInterface;
  };

  return NavDrawer;
}($);

/*
 * selection control focus
 * chrome persists the focus style on checkboxes/radio buttons after clicking
 */

var SelectionControlFocus = function ($$$1) {
  // constants >>>
  var DATA_KEY = 'md.selectioncontrolfocus';
  var EVENT_KEY = "." + DATA_KEY;
  var ClassName = {
    FOCUS: 'focus'
  };
  var LastInteraction = {
    IS_MOUSEDOWN: false
  };
  var Event = {
    BLUR: "blur" + EVENT_KEY,
    FOCUS: "focus" + EVENT_KEY,
    MOUSEDOWN: "mousedown" + EVENT_KEY,
    MOUSEUP: "mouseup" + EVENT_KEY
  };
  var Selector = {
    CONTROL: '.custom-control',
    INPUT: '.custom-control-input' // <<< constants

  };
  $$$1(document).on("" + Event.BLUR, Selector.INPUT, function () {
    $$$1(this).removeClass(ClassName.FOCUS);
  }).on("" + Event.FOCUS, Selector.INPUT, function () {
    if (LastInteraction.IS_MOUSEDOWN === false) {
      $$$1(this).addClass(ClassName.FOCUS);
    }
  }).on("" + Event.MOUSEDOWN, Selector.CONTROL, function () {
    LastInteraction.IS_MOUSEDOWN = true;
  }).on("" + Event.MOUSEUP, Selector.CONTROL, function () {
    setTimeout(function () {
      LastInteraction.IS_MOUSEDOWN = false;
    }, 1);
  });
}($);

/*
 * Tab indicator animation
 * Requires Bootstrap's (v4.0.0) `tab.js`
 */

var TabSwitch = function ($$$1) {
  // constants >>>
  var DATA_KEY = 'md.tabswitch';
  var NAME = 'tabswitch';
  var NO_CONFLICT = $$$1.fn[NAME];
  var Breakpoints = {
    DESKTOP: 992,
    TABLET: 576
  };
  var ClassName = {
    ANIMATE: 'animate',
    DROPDOWN_ITEM: 'dropdown-item',
    INDICATOR: 'nav-tabs-indicator',
    MATERIAL: 'nav-tabs-material',
    SCROLLABLE: 'nav-tabs-scrollable',
    SHOW: 'show'
  };
  var Event = {
    SHOW_BS_TAB: 'show.bs.tab'
  };
  var Selector = {
    DATA_TOGGLE: '.nav-tabs [data-toggle="tab"]',
    DROPDOWN: '.dropdown',
    NAV: '.nav-tabs'
  };
  var TransitionDuration = {
    DESKTOP: 200,
    MOBILE: 300,
    TABLET: 390 // <<< constants

  };

  var TabSwitch =
  /*#__PURE__*/
  function () {
    function TabSwitch(nav) {
      this._nav = nav;
      this._navindicator = null;
    }

    var _proto = TabSwitch.prototype;

    _proto.switch = function _switch(element, relatedTarget) {
      var _this = this;

      var navLeft = $$$1(this._nav).offset().left;
      var navScrollLeft = $$$1(this._nav).scrollLeft();
      var navWidth = $$$1(this._nav).outerWidth();
      var supportsTransition = Util.supportsTransitionEnd();

      if (!this._navindicator) {
        this._createIndicator(navLeft, navScrollLeft, navWidth, relatedTarget);
      }

      if ($$$1(element).hasClass(ClassName.DROPDOWN_ITEM)) {
        element = $$$1(element).closest(Selector.DROPDOWN);
      }

      var elLeft = $$$1(element).offset().left;
      var elWidth = $$$1(element).outerWidth();
      $$$1(this._navindicator).addClass(ClassName.SHOW);
      Util.reflow(this._navindicator);

      if (supportsTransition) {
        $$$1(this._nav).addClass(ClassName.ANIMATE);
      }

      $$$1(this._navindicator).css({
        left: elLeft + navScrollLeft - navLeft,
        right: navWidth - (elLeft + navScrollLeft - navLeft + elWidth)
      });

      var complete = function complete() {
        $$$1(_this._nav).removeClass(ClassName.ANIMATE);
        $$$1(_this._navindicator).removeClass(ClassName.SHOW);
      };

      if (!supportsTransition) {
        complete();
        return;
      }

      var transitionDuration = TransitionDuration.MOBILE;

      if (window.innerWidth >= Breakpoints.DESKTOP) {
        transitionDuration = TransitionDuration.DESKTOP;
      } else if (window.innerWidth >= Breakpoints.TABLET) {
        transitionDuration = TransitionDuration.TABLET;
      }

      $$$1(this._navindicator).one(Util.TRANSITION_END, complete).emulateTransitionEnd(transitionDuration);
    };

    _proto._createIndicator = function _createIndicator(navLeft, navScrollLeft, navWidth, relatedTarget) {
      this._navindicator = document.createElement('div');
      $$$1(this._navindicator).addClass(ClassName.INDICATOR).appendTo(this._nav);

      if (relatedTarget !== 'undefined') {
        if ($$$1(relatedTarget).hasClass(ClassName.DROPDOWN_ITEM)) {
          relatedTarget = $$$1(relatedTarget).closest(Selector.DROPDOWN);
        }

        var relatedLeft = $$$1(relatedTarget).offset().left;
        var relatedWidth = $$$1(relatedTarget).outerWidth();
        $$$1(this._navindicator).css({
          left: relatedLeft + navScrollLeft - navLeft,
          right: navWidth - (relatedLeft + navScrollLeft - navLeft + relatedWidth)
        });
      }

      $$$1(this._nav).addClass(ClassName.MATERIAL);
    };

    TabSwitch._jQueryInterface = function _jQueryInterface(relatedTarget) {
      return this.each(function () {
        var nav = $$$1(this).closest(Selector.NAV)[0];

        if (!nav) {
          return;
        }

        var data = $$$1(nav).data(DATA_KEY);

        if (!data) {
          data = new TabSwitch(nav);
          $$$1(nav).data(DATA_KEY, data);
        }

        data.switch(this, relatedTarget);
      });
    };

    return TabSwitch;
  }();

  $$$1(document).on(Event.SHOW_BS_TAB, Selector.DATA_TOGGLE, function (event) {
    TabSwitch._jQueryInterface.call($$$1(this), event.relatedTarget);
  });
  $$$1.fn[NAME] = TabSwitch._jQueryInterface;
  $$$1.fn[NAME].Constructor = TabSwitch;

  $$$1.fn[NAME].noConflict = function () {
    $$$1.fn[NAME] = NO_CONFLICT;
    return TabSwitch._jQueryInterface;
  };

  return TabSwitch;
}($);

// Extend third party JavaScript

exports.Util = Util;
exports.ExpansionPanel = ExpansionPanel;
exports.FloatingLabel = FloatingLabel;
exports.NavDrawer = NavDrawer;
exports.SelectionControlFocus = SelectionControlFocus;
exports.TabSwitch = TabSwitch;

Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=material.js.map
