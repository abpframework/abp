(function ($) {

    let l = abp.localization.getResource('CmsKit');

    abp.widgets.CmsCommenting = function ($widget) {
        let widgetManager = $widget.data('abp-widget-manager');
        let $commentArea = $widget.find('.cms-comment-area');

        function getFilters() {
            return {
                entityType: $commentArea.attr('data-entity-type'),
                entityId: $commentArea.attr('data-entity-id')
            };
        }

        function isDoubleClicked(element) {
            if (element.data("isclicked")) return true;

            element.data("isclicked", true);
            setTimeout(function () {
                element.removeData("isclicked");
            }, 2000);
        }

        function registerCommentTime($container) {
            $container.find('[comment-time]').each(function () {
                const $timeElement = $(this);

                const creationTime = moment.utc($timeElement.attr('comment-time'));
                var timeAgo = formatTime(creationTime);
                var readableTime = formatReadableTimestamp(creationTime);

                $timeElement.text(timeAgo);
                $timeElement.on('click', function () {
                    if (isDoubleClicked($timeElement)) return;

                    $timeElement.text(readableTime);
                    setTimeout(function () {
                        $timeElement.trigger('focusout');
                    }, 2000);

                });

                $timeElement.on('focusout', function () {
                    $timeElement.text(timeAgo);
                });
            });
        }

        function formatTime(creationTime) {
            let now = moment();
            let duration = moment.duration(now.diff(creationTime));
            if (duration.asMinutes() < 1) {
                return l('JustNow');
            } else if (duration.asMinutes() < 60) {
                return `${Math.floor(duration.asMinutes())} ${l('Minute')}`;
            } else if (duration.asHours() < 24) {
                return `${Math.floor(duration.asHours())} ${l('Hour')}`;
            } else if (duration.asDays() < 7) {
                return `${Math.floor(duration.asDays())} ${l('Day')}`;
            } else {
                return `${Math.floor(duration.asWeeks())} ${l('Week')}`;
            }
        }

        function formatReadableTimestamp(creationTime) {
            const now = moment();
            const diffInMinutes = now.diff(creationTime, 'minutes');
            const diffInHours = now.diff(creationTime, 'hours');
            const diffInDays = now.diff(creationTime, 'days');

            if (diffInMinutes < 1) {
                return l('JustNow');
            } else if (diffInMinutes < 60) {
                return l('MinutesAgo', diffInMinutes);
            } else if (diffInHours < 24) {
                return diffInHours === 1 ? l('HourAgo') : l('HoursAgo', diffInHours);
            } else if (diffInDays === 1) {
                return l('YesterdayAt', creationTime.local().format('h:mm a'));
            } else if (diffInDays < 7) {
                return l('DayAt', creationTime.local().format('dddd'), creationTime.local().format('h:mm a'));
            } else if (now.isSame(creationTime, 'year')) {
                return l('MonthDayAt', creationTime.local().format('D'), creationTime.local().format('MMMM'), creationTime.local().format('h:mm a') );
            } else {
                return l('FullDate', creationTime.local().format('D'), creationTime.local().format('MMMM'), creationTime.local().format('YYYY'));
            }
        }

        function registerEditLinks($container) {
            $container.find('.comment-edit-link').each(function () {
                let $link = $(this);
                $link.on('click', function (e) {
                    e.preventDefault();

                    let commentId = $link.data('id');

                    let $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id=' + commentId + ']');
                    let $relatedCommentEditFormArea = $container.find('.cms-comment-edit-area[data-id=' + commentId + ']');

                    $relatedCommentContentArea.hide();
                    $relatedCommentEditFormArea.show();
                    $link.removeAttr('href');
                });
            });
            $container.find('.comment-edit-cancel-button').each(function () {
                let $button = $(this);
                $button.on('click', function (e) {
                    e.preventDefault();

                    let commentId = $button.data('id');

                    let $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id=' + commentId + ']');
                    let $relatedCommentEditFormArea = $container.find('.cms-comment-edit-area[data-id=' + commentId + ']');
                    let $link = $container.find('.comment-edit-link[data-id=' + commentId + ']');

                    $relatedCommentContentArea.show();
                    $relatedCommentEditFormArea.hide();
                    $link.attr('href', '#');
                });
            });
        }

        function registerReplyLinks($container) {
            $container.find('.comment-reply-link').each(function () {
                let $link = $(this);
                $link.on('click', function (e) {
                    e.preventDefault();

                    let replyCommentId = $link.data('reply-id');

                    let $relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id=' + replyCommentId + ']');
                    let $links = $container.find('.comment-reply-link[data-reply-id=' + replyCommentId + ']');

                    $relatedCommentArea.show();
                    $relatedCommentArea.find('textarea').focus();
                    $links.addClass('disabled');
                });
            });
            $container.find('.reply-cancel-button').each(function () {
                let $button = $(this);
                $button.on('click', function (e) {
                    e.preventDefault();

                    let replyCommentId = $button.data('reply-id');

                    let $relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id=' + replyCommentId + ']');
                    let $links = $container.find('.comment-reply-link[data-reply-id=' + replyCommentId + ']');

                    $relatedCommentArea.hide();
                    $links.removeClass('disabled');
                });
            });
        }

        function registerDeleteLinks($container) {
            $container.find('.comment-delete-link').each(function () {
                let $link = $(this);

                let allowDelete = abp.auth.isGranted('CmsKitPublic.Comments.DeleteAll');
                let isCurrentUser = abp.currentUser.id == $link.data('author-id');
                if (!allowDelete && !isCurrentUser) {
                    $link.hide();
                }
                else {
                    $link.on('click', '', function (e) {
                        e.preventDefault();

                        abp.message.confirm(l("MessageDeletionConfirmationMessage"), function (ok) {
                            if (ok) {
                                volo.cmsKit.public.comments.commentPublic.delete($link.data('id')
                                ).then(function () {
                                    widgetManager.refresh($widget);
                                });
                            }
                        });
                    });
                }
            });
        }

        function registerUpdateOfNewComment($container) {
            $container.find('.cms-comment-update-form').each(function () {
                var $form = $(this);

                $form.submit(function (e) {
                    e.preventDefault();

                    abp.ui.setBusy($form.find("button[type='submit']"));

                    let formAsObject = $form.serializeFormToObject();
                    
                    $.ajax({
                        type: 'POST',
                        url: '/CmsKitPublicComments/Update/' + formAsObject.id,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify({
                            text: formAsObject.commentText,
                            concurrencyStamp: formAsObject.commentConcurrencyStamp,
                            captchaToken: formAsObject.captchaId,
                            captchaAnswer: formAsObject.input?.captcha
                        }),
                        success: function () {
                            widgetManager.refresh($widget);
                            abp.ui.clearBusy();
                        },
                        error: function (data) {
                            abp.message.error(data.responseJSON.error.message);
                            abp.ui.clearBusy();
                        }
                    });
                });
            });
        }

        function registerSubmissionOfNewComment($container) {
            $container.find('.cms-comment-form').each(function () {
                var $form = $(this);

                $form.submit(function (e) {
                    e.preventDefault();

                    abp.ui.setBusy("button[type='submit']");

                    var formAsObject = $form.serializeFormToObject();

                    if (formAsObject.repliedCommentId == '') {
                        formAsObject.repliedCommentId = null;
                    }

                    if (formAsObject.commentText == '') {
                        abp.message.error(l("CommentTextRequired"));
                        abp.ui.clearBusy();
                        return;
                    }

                    $.ajax({
                        type: 'POST',
                        url: '/CmsKitPublicComments/Validate',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify({
                            entityId: $commentArea.attr('data-entity-id'),
                            entityType: $commentArea.attr('data-entity-type'),
                            repliedCommentId: formAsObject.repliedCommentId,
                            text: formAsObject.commentText,
                            url: window.location.href,
                            captchaToken: formAsObject.captchaId,
                            captchaAnswer: formAsObject.input?.captcha,
                            idempotencyToken: formAsObject.idempotencyToken
                        }),
                        success: function () {
                            widgetManager.refresh($widget);
                            if (abp.setting.getBoolean("CmsKit.Comments.RequireApprovement")) {
                                abp.message.success(l("CommentSubmittedForApproval"), l("SavedSuccessfully"));
                            }
                            $form.trigger('reset');
                            abp.ui.clearBusy();
                        },
                        error: function (data) {
                            abp.message.error(data.responseJSON.error.message);
                            abp.ui.clearBusy();
                        }
                    });
                });
            });
        }

        function focusOnHash($container) {
            if (!location.hash.toLowerCase().startsWith('#cms-comment')) {
                return;
            }

            let $link = $(location.hash + '_link');

            if ($link.length > 0) {
                $link.click();
            }
            else {
                $(location.hash).find('textarea').focus();
            }
        }
        
        function init() {
            registerReplyLinks($widget);
            registerEditLinks($widget);
            registerDeleteLinks($widget);

            registerUpdateOfNewComment($widget);
            registerSubmissionOfNewComment($widget);

            registerCommentTime($widget);

            focusOnHash($widget);
        }

        return {
            init: init,
            getFilters: getFilters
        };
    };

})(jQuery);
