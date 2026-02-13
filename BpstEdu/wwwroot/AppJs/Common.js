var common = {
    jsonToQueryString: function (json) {
        return Object.keys(json)
            .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(json[key]))
            .join('&');
    },
    GetApplications: function () {
        $.ajax({
            url: "/staff/Applications/GetApplicationPartials/",
            data: { _courseId: $('#CourseDDL').val(), _statusId: $('#RegStatusDDL').val() },
            success: function (response) {
                $("#applicationsDiv").html(response);  // Insert the HTML response into the #content div
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: " + status + " - " + error);
            }
        });
    },
    loadFeePartial: function (_batchId, _studentId) {
        $.ajax({
            url: "/staff/FeeSubmission/Create/",
            data: { batchId: _batchId, studentId: _studentId },
            success: function (response) {
                $("#FeeModalPopUp").html(response);
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error: " + status + " - " + error);
            }
        });
    },

    EnrollToBatch: function (_appId) {
        if ($('#BatchDDL').val() == 0) {
            alert("Select Batch First.");
            return;
        }

        var _data = { _applicationId: _appId, _batchId: $('#BatchDDL').val(), };
        var _parameter = common.jsonToQueryString(_data);
        var _url = '/staff/Applications/EnrollToBatch?' + _parameter;
        window.location.href = _url;
    },
    GetBatchesByCourseId: function (ddl) {
        var courseId = $(ddl).val();
        $.ajax({
            type: "GET",
            url: '/Home/GetBatchesByCourseId?courseId=' + courseId,
            cache: false,
            success: function (data) {
                if ($.isEmptyObject(data)) {
                    $('#BatchDDL').parent().hide();
                }
                else {
                    var items = '<select class="custom-select form-control">';
                    $.each(data, function (i, batch) {
                        items += '<option value="' + batch.id + '">' + batch.value + '</option>'
                    });
                    items += '</select>';
                    $('#BatchDDL').html(items);
                    $('#BatchDDL').parent().show();
                }
            }
        });
    },

}