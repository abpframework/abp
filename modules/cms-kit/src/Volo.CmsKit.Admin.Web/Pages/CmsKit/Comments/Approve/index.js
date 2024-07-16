$(function () {
    var l = abp.localization.getResource("CmsKit");

    var commentsService = volo.cmsKit.admin.comments.commentAdmin;

    moment()._locale.preparse = (string) => string;
    moment()._locale.postformat = (string) => string;

    var getFormattedDate = function ($datePicker) {
        if (!$datePicker.val()) {
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
        filterObj.commentApproveState = "Waiting";

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
                visible: abp.auth.isGranted('CmsKit.Comments.Update'),
                render: function (data, type, row) {
                    let approveButton = $(`<button class="btn btn-xs btn-success change-status text-light" data-id="${row.id}" data-status="true" data-toggle="tooltip" data-placement="top" title="${l('Approve')}"><i class="fa fa-check"></i></button>`);
                    let rejectButton = $(`<button class="btn btn-xs btn-danger change-status text-light" data-id="${row.id}" data-status="false" data-toggle="tooltip" data-placement="top" title="${l('Disapproved')}"><i class="fa fa-times"></i></button>`);
                    let buttons = [];
                    buttons.push(approveButton);
                    buttons.push(rejectButton);

                    let result = `<div class="btn-group">`;

                    buttons.forEach(x => {
                        if(x.data("status") !== data) {
                            result += x.get(0).outerHTML;
                        }
                    } );

                    result += `</div>`;
                    return result;
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
                        return '<a href="' + data + '#comment-' + row.id + '" target="_blank"><i class="fa fa-location-arrow"></i></a>';
                    }
                    return "";
                }
            },
            {
                title: l("Text"),
                data: "text",
                orderable: false,
                render: function (data) {
                    var md = window.markdownit();
                    var htmlContent = md.render(data);
                    return (
                        '<span data-toggle="tooltip" title="' +
                        htmlContent +
                        '" style="display: block; white-space: normal; word-break: break-all; max-width: 100%; overflow: hidden;">' +
                        htmlContent +
                        "</span>"
                    );
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

    function GetFilterableDatatableContent(filterSelector, data) {
        return '<span class="datatableCell" data-field="' + filterSelector + '" data-val="' + data + '">' + data + '</span>';
    }

    $(document).on('click', '.datatableCell', function () {
        var inputSelector = $(this).attr('data-field');
        var value = $(this).attr('data-val');

        $(inputSelector).val(value);

        _dataTable.ajax.reload();
    });

    filterForm.submit(function (e) {
        e.preventDefault();
        _dataTable.ajax.reload();
    });
    
    _dataTable.on('draw.dt', function () {
        var changeStatusButton = $(".change-status");
        changeStatusButton.click(function () {
            $(this).html("<i class='fa fa-spinner fa-spin'></i>");
            $(this).prop("disabled", true);

            let id = $(this).data("id");
            let isApproved = $(this).data("status");

            commentsService
                .updateApprovalStatus(id, {IsApproved: isApproved})
                .then(function (data) {
                    var message = isApproved ? l('ApprovedSuccessfully') : l('ApprovalRevokedSuccessfully');
                    abp.notify.success(message);
                    _dataTable.ajax.reload();
                })
        });
    });
});
