﻿(function ($) {

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

            function registerEditLinks($container) {
                $container.find('.comment-edit-link').each(function () {
                    var $link = $(this);
                    $link.on('click', function (e) {
                        e.preventDefault();

                        var commentId = $link.data('id');

                        var $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id='+ commentId +']');
                        var $relatedCommentEditFormArea = $container.find('.cms-comment-edit-area[data-id='+ commentId +']');

                        $relatedCommentContentArea.hide();
                        $relatedCommentEditFormArea.show();
                        $link.removeAttr('href');
                    });
                });
                $container.find('.comment-edit-cancel-button').each(function () {
                    var $button = $(this);
                    $button.on('click', function (e) {
                        e.preventDefault();

                        var commentId = $button.data('id');

                        var $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id='+ commentId +']');
                        var $relatedCommentEditFormArea = $container.find('.cms-comment-edit-area[data-id='+ commentId +']');
                        var $link = $container.find('.comment-edit-link[data-id='+ commentId +']');

                        $relatedCommentContentArea.show();
                        $relatedCommentEditFormArea.hide();
                        $link.attr('href','#');
                    });
                });
            }

            function registerReplyLinks($container) {
                $container.find('.comment-reply-link').each(function () {
                    var $link = $(this);
                    $link.on('click', function (e) {
                        e.preventDefault();

                        var replyCommentId = $link.data('reply-id');

                        var $relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id='+ replyCommentId +']');

                        $relatedCommentArea.show();
                        $link.removeAttr('href');
                    });
                });
                $container.find('.reply-cancel-button').each(function () {
                    var $button = $(this);
                    $button.on('click', function (e) {
                        e.preventDefault();

                        var replyCommentId = $button.data('reply-id');

                        var $relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id='+ replyCommentId +']');
                        var $replyLink = $container.find('.comment-reply-link[data-reply-id='+ replyCommentId +']');

                        $relatedCommentArea.hide();
                        $replyLink.attr('href','#');
                    });
                });
            }

            function registerDeleteLinks($container) {
                $container.find('.comment-delete-link').each(function () {
                    var $link = $(this);
                    $link.on('click', '', function (e) {
                        e.preventDefault();

                        abp.message.confirm(l("MessageDeletionConfirmationMessage"), function (ok) {
                            if (ok){
                                volo.cmsKit.public.comments.commentPublic.delete($link.data('id')
                                ).then(function () {
                                    widgetManager.refresh($widget);
                                });
                            }
                        });
                    });
                });
            }

            function registerUpdateOfNewComment($container) {
                $container.find('.cms-comment-update-form').each(function () {
                    var $form = $(this);
                    $form.submit(function (e) {
                        e.preventDefault();
                        var formAsObject = $form.serializeFormToObject();
                        volo.cmsKit.public.comments.commentPublic.update(
                            formAsObject.id,
                            {
                                text: formAsObject.commentText
                            }
                        ).then(function () {
                            widgetManager.refresh($widget);
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
                        volo.cmsKit.public.comments.commentPublic.create(
                            $.extend(getFilters(), {
                                repliedCommentId: formAsObject.repliedCommentId,
                                text: formAsObject.commentText
                            })
                        ).then(function () {
                            widgetManager.refresh($widget);
                        });
                    });
                });
            }

            function init() {
                registerReplyLinks($widget);
                registerEditLinks($widget);
                registerDeleteLinks($widget);

                registerUpdateOfNewComment($widget);
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
