(function ($) {

    var l = abp.localization.getResource('CmsKit');

    $(document).ready(function () {

        abp.widgets.CmsCommenting = function ($widget) {
            var widgetManager = $widget.data('abp-widget-manager');
            var $commentArea = $widget.find('.cms-comment-area');

            function getFilters() {
                return {
                    entityType: $commentArea.attr('data-entity-type'),
                    entityId: $commentArea.attr('data-entity-id')
                };
            }

            function registerReplyLinks($container) {
                $container.find('.comment-reply-link').each(function () {
                    var $link = $(this);
                    $link.on('click', function (e) {
                        e.preventDefault();

                        var replyCommentId = $link.data('reply-id');

                        var relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id='+ replyCommentId +']');

                        relatedCommentArea.show();
                    });
                });
                $container.find('.reply-cancel-button').each(function () {
                    var $button = $(this);
                    $button.on('click', function (e) {
                        e.preventDefault();

                        var replyCommentId = $button.data('reply-id');

                        var relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id='+ replyCommentId +']');

                        relatedCommentArea.hide();
                    });
                });
            }

            function registerDeleteLinks($container) {
                $container.find('.comment-delete-link').each(function () {
                    var $link = $(this);
                    $link.on('click', '', function (e) {
                        e.preventDefault();

                        abp.message.confirm(l("MessageDeletionConfirmationMessage"), function () {
                            volo.cmsKit.comments.commentPublic.delete($link.data('comment-id')
                            ).then(function () {
                                widgetManager.refresh($widget);
                            });
                        });
                    });
                });
            }

            function registerSubmissionOfNewComment($container) {
                $container.find('.cms-comment-form').each(function () {
                    var $form = $(this);
                    $form.submit(function (e) {
                        e.preventDefault();
                        var formAsObject = $form.serializeFormToObject();
                        volo.cmsKit.comments.commentPublic.create(
                            $.extend(getFilters(), {
                                repliedCommentId: formAsObject.repliedCommentId,
                                text: formAsObject.commentText
                            })
                        ).then(function () {
                            $form.find('textarea').val('');
                            widgetManager.refresh($widget);
                        });
                    });
                });
            }

            function init() {
                registerReplyLinks($widget);
                registerDeleteLinks($widget);
                registerSubmissionOfNewComment($widget);
            }

            return {
                init: init,
                getFilters: getFilters
            };
        };

        $('.abp-widget-wrapper[data-widget-name="CmsCommenting"]')
            .each(function () {
                var widgetManager = new abp.WidgetManager({
                    wrapper: $(this),
                });

                widgetManager.init();
            });
    });

})(jQuery);
