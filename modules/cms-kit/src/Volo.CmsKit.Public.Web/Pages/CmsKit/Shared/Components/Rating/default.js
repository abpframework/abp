(function () {
    var l = abp.localization.getResource("CmsKit");

    abp.widgets.CmsRating = function ($widget) {
        var widgetManager = $widget.data("abp-widget-manager");
        var $ratingArea = $widget.find(".cms-rating-area");

        function getFilters() {
            return {
                entityType: $ratingArea.attr("data-entity-type"),
                entityId: $ratingArea.attr("data-entity-id")
            };
        }

        function registerCreateOfNewRating() {
            $widget.find(".my-rating").each(function () {
                    var authenticated = $(this).attr("data-authenticated");
                    var readonly = $(this).attr("data-readonly");
                    var currentRating = parseInt($(this).attr("data-rating")) || 0;
                    var $liveRating = $widget.find(".live-rating");
                    var averageRating = $liveRating.text();

                    $(this).starRating({
                        initialRating: currentRating,
                        starSize: 16,
                        emptyColor: '#eee',
                        hoverColor: '#ffc107',
                        activeColor: '#ffc107',
                        useGradient: false,
                        strokeWidth: 0,
                        disableAfterRate: false,
                        useFullStars: true,
                        readOnly: authenticated === "True" || readonly === "True",

                        callback: function (currentRating, $el) {
                            volo.cmsKit.public.ratings.ratingPublic.create(
                                $ratingArea.attr("data-entity-type"),
                                $ratingArea.attr("data-entity-id"),
                                {
                                    starCount: parseInt(currentRating)
                                }
                            ).then(function () {
                                widgetManager.refresh($widget);
                            })
                        }
                    });
                }
            );
        }

        function init() {
            registerCreateOfNewRating();
        }

        return {
            init: init,
            getFilters: getFilters
        }
    };
})
(jQuery);
