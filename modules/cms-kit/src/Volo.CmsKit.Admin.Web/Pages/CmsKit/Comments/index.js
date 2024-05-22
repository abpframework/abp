$(function (){
    var l = abp.localization.getResource("CmsKit");
    
    var commentsService = volo.cmsKit.admin.comments.commentAdmin;

    var detailsModal = new abp.ModalManager(abp.appPath + "CmsKit/Comments/DetailsModal");
    
    moment()._locale.preparse = (string) => string;
    moment()._locale.postformat = (string) => string;
    
    var getFormattedDate = function ($datePicker) {
        if(!$datePicker.val()) {
            return null;
        }
        var momentDate = moment($datePicker.val(), $datePicker.data('daterangepicker').locale.format);
        return momentDate.isValid() ? momentDate.toISOString() : null;
    };
    

    $('.singledatepicker').daterangepicker({
        "singleDatePicker": true,
        "showDropdowns": true,
        "autoUpdateInput": false,
        "autoApply": true,
        "opens": "center",
        "drops": "auto"
    });

    

    $('.singledatepicker').attr('autocomplete', 'off');

    $('.singledatepicker').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('l'));
    });
    
    
    var filterForm = $('#CmsKitCommentsFilterForm');
    
    var getFilter = function () {
        var filterObj = filterForm.serializeFormToObject();

        filterObj.creationStartDate = getFormattedDate($('#CreationStartDate'));
        filterObj.creationEndDate = getFormattedDate($('#CreationEndDate'));
        
        return filterObj;
    };
  
    
    var _dataTable = $('#CommentsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        scrollX: true,
        searching: false,
        scrollCollapse: true,
        ajax: abp.libs.datatables.createAjax(commentsService.getList, getFilter),
        columnDefs: [
            {
                width: "10%",
                title: l("Actions"),
                targets: 0,
                orderable: false,
                rowAction: {
                    items: [
                        {
                            text: l('Details'),
                            action: function (data) {
                                location.href = 'Comments/' + data.record.id;
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.Comments.Delete'),
                            confirmMessage: function (data) {
                                return l("CommentDeletionConfirmationMessage")
                            },
                            action: function (data) {
                                commentsService
                                    .delete(data.record.id)
                                    .then(function () {
                                        _dataTable.ajax.reloadEx();
                                        abp.notify.success(l('DeletedSuccessfully'));
                                    });
                            }
                        },
                        {
                            text: function (data) {
                                return data.isApproved ? l('Disapproved') : l('Approve');
                            },
                            action: function (data) {
                                var newApprovalStatus = !data.record?.isApproved;

                                commentsService
                                    .updateApprovalStatus(data.record.id, { IsApproved: newApprovalStatus })
                                    .then(function () {
                                        _dataTable.ajax.reloadEx();
                                        var message = newApprovalStatus ? l('ApprovedSuccessfully') : l('ApprovalRevokedSuccessfully');
                                        abp.notify.success(message);
                                    })
                                    .catch(function (error) {
                                        abp.notify.error(error.message);
                                    });
                            }
                        },
                        {
                            text: function (data) {
                                if (data.isApproved == null) {
                                    return l('Disapproved')
                                }
                            },
                            action: function (data) {
                                var newApprovalStatus = false;

                                commentsService
                                    .updateApprovalStatus(data.record.id, { IsApproved: newApprovalStatus })
                                    .then(function () {
                                        _dataTable.ajax.reloadEx();
                                        var message = newApprovalStatus ? l('ApprovedSuccessfully') : l('ApprovalRevokedSuccessfully');
                                        abp.notify.success(message);
                                    })
                                    .catch(function (error) {
                                        abp.notify.error(error.message);
                                    });
                            }
                        }
                    ]
                }
            },
            {
                width: "10%",
                title: l("Username"),
                orderable: false,
                data: "author.userName",
                render: function (data) {
                    if (data !== null) {
                        return GetFilterableDatatableContent('#Author', $.fn.dataTable.render.text().display(data)); //prevent against possible XSS
                    }
                    return "";
                }
            },
            {
                width: "15%",
                title: l("EntityType"),
                orderable: false,
                data: "entityType",
                render: function (data) {
                    if (data !== null) {
                        return GetFilterableDatatableContent('#EntityType', $.fn.dataTable.render.text().display(data));
                    }
                    return "";
                }
            },
            {
                title: l("URL"),
                data: "url",
                render: function (data, type, row) {
                    if (data !== null) {
                        return '<a href="' + data + '#comment-'+ row.id + '" target="_blank"><i class="fa fa-location-arrow"></i></a>';
                    }
                    return "";
                }
            },
            {
                title: l("Text"),
                data: "text",
                orderable: false,
                render: function (data) {
                    data = $.fn.dataTable.render.text().display(data || "");

                    var maxChars = 64;

                    if (data.length > maxChars) {
                        return (
                            '<span data-toggle="tooltip" title="' +
                            data +
                            '">' +
                            data.substring(0, maxChars) +
                            "..." +
                            "</span>"
                        );
                    } else {
                        return data;
                    }
                }
            },
            {
                width: "15%",
                title: l("CreationTime"),
                data: "creationTime",
                orderable: true,
                dataFormat: "datetime"
            },
            {
                width: "5%",
                title: l("Status"),
                orderable: false,

                data: "isApproved",
                render: function (data, type, row) {
                    var icons = ''

                    if (data === null) {
                        icons = '<i class="fa-solid fa-hourglass-start"></i>';
                    } else if (typeof data === "boolean") {
                        if (data) {
                            icons = '<i class="fa-solid fa-check" style="color: #63E6BE;"></i>';
                        } else {
                            icons = '<i class="fa-solid fa-x" style="color: #e0102f;"></i>';
                        }
                    }

                    return icons;
                }
            }
        ]
    }));
    
    function GetFilterableDatatableContent(filterSelector, data){
        return '<span class="datatableCell" data-field="'+ filterSelector +'" data-val="'+ data +'">' + data + '</span>';
    }
    
    $(document).on('click', '.datatableCell', function () {
        var inputSelector = $(this).attr('data-field');
        var value = $(this).attr('data-val');
        
        $(inputSelector).val(value);
        
        _dataTable.ajax.reloadEx();
    });

    filterForm.submit(function (e){
        e.preventDefault();
        _dataTable.ajax.reloadEx();
    });

    // Get and display waiting comment count
    commentsService.getWaitingCommentCount().then(function (count) {
        if (count > 0) {
            var alertMessage = l("CommentAlertMessage", count);
            var alertElement = '<abp-alert alert-type="Warning">' + alertMessage + '</abp-alert>';
            $('#commentsAlert').html(alertElement);
            $('#commentsAlert').show()
            $('#commentsAlert').click(function () {
                window.location.href = '/Cms/Comments/Approve'
            });
        }
    });

    //
    commentsService.getSettings().then(function (data) {
        if (data.requireApprovement) {
            $('#CommentsTable').DataTable().column(6).visible(true);
        } else {
            $('#CommentsTable').DataTable().column(6).visible(false);
            $('#isApprovedColumn').hide();
        }
    })
});