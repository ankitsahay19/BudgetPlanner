var StudentApplications = {
    GetStudentByCourseOrBoardName: function () {
        var BoardId = $("#BoardId").val();
        var ApplicationFor = $("#ApplicationFor").val();
        $.ajax({
            type: "POST",
            url: "/Admin/Registrations/GetStudentFilter",
            data: { BoardId: BoardId, ApplicationFor: ApplicationFor },
            cache: false,
            success: function (data) {
                $('#StudentDiv').html(data);
                $(".table").DataTable();
            },
            error: function (data) {
                console.log(data);
            }
        });
    },
    FeeEvaluation: function () {
        debugger;
        var TotalFees = $("#TotalFees").val();
        var Discount = $("#Discount").val();
        var RemainingFees = $("#RemainingFees").val();
        var InstallmentFees = $("#InstallmentFees").val();

        if (TotalFees >= 0 && Discount >= 0) {
            var remainFee = TotalFees - Discount;
            $("#RemainingFees").val(remainFee);
        }
        if (remainFee >= 0) {

        }
    }
}