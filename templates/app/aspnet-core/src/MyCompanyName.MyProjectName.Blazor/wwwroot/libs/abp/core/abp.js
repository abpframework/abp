window.abp.utils.setCookieValue = function (key, value, expireDate, path) {
    var cookieValue = encodeURIComponent(key) + '=';
    if (value) {
        cookieValue = cookieValue + encodeURIComponent(value);
    }

    if (expireDate) {
        cookieValue = cookieValue + "; expires=" + expireDate;
    }

    if (path) {
        cookieValue = cookieValue + "; path=" + path;
    }

    document.cookie = cookieValue;
};

/**
 * Gets a cookie with given key.
 * This is a simple implementation created to be used by ABP.
 * Please use a complete cookie library if you need.
 * @param {string} key
 * @returns {string} Cookie value or null
 */
window.abp.utils.getCookieValue = function (key) {
    var equalities = document.cookie.split('; ');
    for (var i = 0; i < equalities.length; i++) {
        if (!equalities[i]) {
            continue;
        }

        var splitted = equalities[i].split('=');
        if (splitted.length != 2) {
            continue;
        }

        if (decodeURIComponent(splitted[0]) === key) {
            return decodeURIComponent(splitted[1] || '');
        }
    }

    return null;
};

/**
 * Deletes cookie for given key.
 * This is a simple implementation created to be used by ABP.
 * Please use a complete cookie library if you need.
 * @param {string} key
 * @param {string} path (optional)
 */
window.abp.utils.deleteCookie = function (key, path) {
    var cookieValue = encodeURIComponent(key) + '=';

    cookieValue = cookieValue + "; expires=" + (new Date(new Date().getTime() - 86400000)).toUTCString();

    if (path) {
        cookieValue = cookieValue + "; path=" + path;
    }

    document.cookie = cookieValue;
}