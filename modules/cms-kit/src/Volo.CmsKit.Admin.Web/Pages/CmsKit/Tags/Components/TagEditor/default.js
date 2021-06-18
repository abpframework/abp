﻿$(function () {

    var $tagEditorForms = $('.tag-editor-form');

    $tagEditorForms.on('submit', function (e) {
        e.preventDefault();

        var $form = $(e.currentTarget);

        if ($form.valid()) {

            abp.ui.setBusy();

            var entityId = $form.data('entity-id');
            var entityType = $form.data('entity-type');
            var tags = $form.find('input').val().split(",");

            volo.cmsKit.admin.tags.entityTagAdmin
                .setEntityTags({
                    entityId: entityId,
                    entityType: entityType,
                    tags: tags
                })
        }
    })
})