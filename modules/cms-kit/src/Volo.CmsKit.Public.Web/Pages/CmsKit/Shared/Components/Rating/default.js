(function () {
    var l = abp.localization.getResource("CmsKit");

    $(document).ready(function () {
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
                $widget.find(".my-rating-5").each(function () {
                        var authenticated = $(this).attr("data-authenticated");

                        $(this).starRating({
                            initialRating: 0,
                            disableAfterRate: true,
                            useFullStars: true,
                            readOnly: authenticated === "True",
                            onHover: function (currentIndex, currentRating, $el) {
                                $widget.find(".live-rating").text(currentIndex);
                            },
                            onLeave: function (currentIndex, currentRating, $el) {
                                $widget.find(".live-rating").text(currentRating);
                            },
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

            function registerUndoLink() {
                $widget.find(".rating-undo-link").each(function () {
                    $(this).on('click', '', function (e) {
                        e.preventDefault();

                        abp.message.confirm(l("RatingUndoMessage"), function (ok) {
                            if (ok) {
                                volo.cmsKit.public.ratings.ratingPublic.delete(
                                    $ratingArea.attr("data-entity-type"),
                                    $ratingArea.attr("data-entity-id")
                                ).then(function () {
                                    widgetManager.refresh($widget);
                                });
                            }
                        })
                    });
                });
            }

            function init() {
                registerCreateOfNewRating();
                registerUndoLink();
            }

            return {
                init: init,
                getFilters: getFilters
            }
        };

        $('.abp-widget-wrapper[data-widget-name="CmsRating"]')
            .each(function () {
                var widgetManager = new abp.WidgetManager({
                    wrapper: $(this),
                });

                widgetManager.init($(this));
            });
    });
})
(jQuery);