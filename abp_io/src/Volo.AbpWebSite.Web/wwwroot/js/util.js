var UTIL = require("n-util");

for (var key in UTIL) {
    exports[key] = UTIL[key];
}

exports.object.deepDiff = function() {
    var sources = Array.prototype.slice.call(arguments);
    var target = exports.deepCopy(sources.shift());
    if (typeof target !== "object") {
        throw new Error("`target` must be an object!");
    }
    return variadicHelper([target].concat(sources), function(target, source) {
        if (typeof source === "object") {
            var key;
            for (key in source) {
                if (exports.object.has(source, key)) {
                    if (exports.object.has(target, key)) {
                        if (exports.deepEqual(target[key], source[key])) {
                            delete target[key];
                        } else {
                            if (exports.isArrayLike(target[key]) && exports.isArrayLike(source[key])) {
                                target[key] = exports.array.diff(target[key], source[key]);
                            } else
                            if (typeof target[key] === "object" && typeof source[key] === "object") {
                                target[key] = exports.deepDiff(target[key], source[key]);
                            }
                        }
                    }
                }
            }
        }
    });
};

exports.array.diff = function(target, source) {
    var sources = Array.prototype.slice.call(arguments);
    var target = exports.deepCopy(sources.shift());
    sources.forEach(function(source) {
        target = target.filter(function(i) {
            return !(source.indexOf(i) > -1);
        });
    });
    return target;
};

exports.deepDiff = exports.operator('deepDiff', 2, function(target, source) {
    var args = Array.prototype.slice.call(arguments);
    return exports.object.deepDiff.apply(this, args);
});

exports.deepEqual = function(actual, expected) {

    // 7.1. All identical values are equivalent, as determined by ===.
    if (actual === expected) {
        return true;

        // 7.2. If the expected value is a Date object, the actual value is
        // equivalent if it is also a Date object that refers to the same time.
    } else if (actual instanceof Date && expected instanceof Date) {
        return actual.getTime() === expected.getTime();

        // 7.3. Other pairs that do not both pass typeof value == "object",
        // equivalence is determined by ==.
    } else if (typeof actual != 'object' && typeof expected != 'object') {
        return actual == expected;

        // XXX specification bug: this should be specified
    } else if (typeof expected == "string" || typeof actual == "string") {
        return expected == actual;

        // 7.4. For all other Object pairs, including Array objects, equivalence is
        // determined by having the same number of owned properties (as verified
        // with Object.prototype.hasOwnProperty.call), the same set of keys
        // (although not necessarily the same order), equivalent values for every
        // corresponding key, and an identical "prototype" property. Note: this
        // accounts for both named and indexed properties on Arrays.
    } else {
        return actual.prototype === expected.prototype && exports.object.eq(actual, expected);
    }
}

/**
 * @param args Arguments list of the calling function
 * First argument should be a callback that takes target and source parameters.
 * Second argument should be target.
 * Remaining arguments are treated a sources.
 *
 * @returns Target
 * @type Object
 */
var variadicHelper = function(args, callback) {
    var sources = Array.prototype.slice.call(args);
    var target = sources.shift();

    sources.forEach(function(source) {
        callback(target, source);
    });

    return target;
};