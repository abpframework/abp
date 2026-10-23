var abp = abp || {};
(function ($) {

    if (typeof timeago === 'undefined') {
        throw "abp/timeago library requires the timeago.js library included to the page!";
    }

    abp.timeago = abp.timeago || {};

    // ABP culture name -> timeago.js locale name (only the ones that don't match the two-letter language code)
    abp.timeago.localeMap = {
        'en': 'en_US',
        'zh': 'zh_CN',
        'zh-Hans': 'zh_CN',
        'zh-Hant': 'zh_TW',
        'zh-CN': 'zh_CN',
        'zh-TW': 'zh_TW',
        'pt': 'pt_BR',
        'nb': 'nb_NO',
        'no': 'nb_NO',
        'nn': 'nn_NO',
        'hi': 'hi_IN',
        'id': 'id_ID',
        'bn': 'bn_IN'
    };

    abp.timeago.getLocale = function (cultureName) {
        cultureName = cultureName || (abp.localization && abp.localization.currentCulture && abp.localization.currentCulture.cultureName) || 'en';

        if (abp.timeago.localeMap[cultureName]) {
            return abp.timeago.localeMap[cultureName];
        }

        var language = cultureName.split('-')[0];
        return abp.timeago.localeMap[language] || language;
    };

    abp.timeago.format = function (date, options) {
        return timeago.format(date, abp.timeago.getLocale(), options);
    };

    var toNodeList = function (nodes) {
        if (!nodes) {
            return [];
        }

        return Array.prototype.filter.call(nodes.nodeType ? [nodes] : nodes, function (node) {
            return node && node.getAttribute;
        });
    };

    // <time> elements carry the date in the "datetime" attribute, other elements may carry it in "title"
    var getDateAttribute = function (node) {
        var datetime = node.getAttribute('datetime');
        if (datetime || node.tagName === 'TIME') {
            return datetime;
        }

        return node.getAttribute('title');
    };

    abp.timeago.render = function (nodes, options) {
        var nodeList = toNodeList(nodes).filter(function (node) {
            var datetime = getDateAttribute(node);
            if (!datetime) {
                return false;
            }

            node.setAttribute('datetime', datetime);
            return true;
        });

        if (!nodeList.length) {
            return nodeList;
        }

        return timeago.render(nodeList, abp.timeago.getLocale(), options);
    };

    abp.timeago.cancel = function (nodes) {
        if (nodes === undefined) {
            timeago.cancel();
            return;
        }

        toNodeList(nodes).forEach(function (node) {
            timeago.cancel(node);
        });
    };

    // Locales that ship with ABP's default languages but are missing in timeago.full.min.js
    var slavicIndex = function (number, index) {
        return (index % 2 === 1 && number >= 5) ? 1 : 0;
    };

    timeago.register('cs', function (number, index) {
        return [
            [['právě teď', 'právě teď']],
            [['před %s vteřinami', 'za %s vteřiny'], ['před %s vteřinami', 'za %s vteřin']],
            [['před minutou', 'za minutu']],
            [['před %s minutami', 'za %s minuty'], ['před %s minutami', 'za %s minut']],
            [['před hodinou', 'za hodinu']],
            [['před %s hodinami', 'za %s hodiny'], ['před %s hodinami', 'za %s hodin']],
            [['včera', 'zítra']],
            [['před %s dny', 'za %s dny'], ['před %s dny', 'za %s dnů']],
            [['minulý týden', 'příští týden']],
            [['před %s týdny', 'za %s týdny'], ['před %s týdny', 'za %s týdnů']],
            [['minulý měsíc', 'příští měsíc']],
            [['před %s měsíci', 'za %s měsíce'], ['před %s měsíci', 'za %s měsíců']],
            [['před rokem', 'příští rok']],
            [['před %s lety', 'za %s roky'], ['před %s lety', 'za %s let']]
        ][index][slavicIndex(number, index)];
    });

    timeago.register('sk', function (number, index) {
        return [
            [['práve teraz', 'práve teraz']],
            [['pred %s sekundami', 'o %s sekundy'], ['pred %s sekundami', 'o %s sekúnd']],
            [['pred minútou', 'o minútu']],
            [['pred %s minútami', 'o %s minúty'], ['pred %s minútami', 'o %s minút']],
            [['pred hodinou', 'o hodinu']],
            [['pred %s hodinami', 'o %s hodiny'], ['pred %s hodinami', 'o %s hodín']],
            [['pred %s dňom', 'o %s deň']],
            [['pred %s dňami', 'o %s dni'], ['pred %s dňami', 'o %s dní']],
            [['pred %s týždňom', 'o %s týždeň']],
            [['pred %s týždňami', 'o %s týždne'], ['pred %s týždňami', 'o %s týždňov']],
            [['pred %s mesiacom', 'o %s mesiac']],
            [['pred %s mesiacmi', 'o %s mesiace'], ['pred %s mesiacmi', 'o %s mesiacov']],
            [['pred %s rokom', 'o %s rok']],
            [['pred %s rokmi', 'o %s roky'], ['pred %s rokmi', 'o %s rokov']]
        ][index][slavicIndex(number, index)];
    });

    if (!$) {
        return;
    }

    $.timeago = function (date) {
        if (date && date.jquery) {
            date = date[0];
        }

        if (date && date.nodeType === 1) {
            date = getDateAttribute(date);
        }

        return abp.timeago.format(date);
    };

    $.fn.timeago = function (action, options) {
        if (action === 'dispose') {
            abp.timeago.cancel(this.toArray());
            return this;
        }

        if (action === 'update' && options !== undefined && options !== null) {
            this.attr('datetime', options instanceof Date ? options.toISOString() : options);
        }

        abp.timeago.render(this.toArray());
        return this;
    };

})(window.jQuery);
