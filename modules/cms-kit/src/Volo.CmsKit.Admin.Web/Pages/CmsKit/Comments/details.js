$(function (){
    var l = abp.localization.getResource("CmsKit");
    
    var commentsService = volo.cmsKit.admin.comments.commentAdmin;

    var detailsModal = new abp.ModalManager(abp.appPath + "CmsKit/Comments/DetailsModal");
    
    $(".input-daterange")
        .datepicker({
            todayBtn: "linked",
            autoclose: true,
            language: abp.localization.currentCulture.cultureName,
        })
        .on("hide", function (e) {
            e.stopPropagation();
        });
    
    var filterForm = $('#CmsKitCommentsFilterForm');
    
    var getFilter = function () {
        var filterObj = filterForm.serializeFormToObject();

        var startDate = luxon.DateTime.fromFormat(
            filterObj.creationStartDate,
            abp.localization.currentCulture.dateTimeFormat.shortDatePattern,
            { locale: abp.localization.currentCulture.cultureName }
        );
        if (!startDate.invalid) {
            filterObj.creationStartDate = startDate.toFormat("yyyy-MM-dd");
        }

        var endDate = luxon.DateTime.fromFormat(
            filterObj.creationEndDate,
            abp.localization.currentCulture.dateTimeFormat.shortDatePattern,
            { locale: abp.localization.currentCulture.cultureName }
        );
        if (!endDate.invalid) {
            filterObj.creationEndDate = endDate.toFormat("yyyy-MM-dd");
        }
        
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
                                window.location = abp.appPath + 'CmsKit/Comments/Details/' + data.record.id;
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
                                        abp.notify.info(l("SuccessfullyDeleted"));
                                        _dataTable.ajax.reload();
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
                        return GetFilterableDatatableContent('#Author', data);
                    }
                    return "";
                }
            },
            {
                title: l("Text"),
                data: "text",
                orderable: false,
                render: function (data) {
                    data = data || "";

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
        
        _dataTable.ajax.reload();
    });

    filterForm.submit(function (e){
        e.preventDefault();
        _dataTable.ajax.reload();
    });
});