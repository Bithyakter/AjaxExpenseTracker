function CategoryDropDown() {
    var Categorydropdown = $("#expenseCategory");

    $.ajax({
        url: APIUrl + "categories",
        cache: false,
        type: "GET",
        dataType: "json",
        contentType: 'application/json',
        success: function (response) {
            Categorydropdown.append($("<option />").val("").text("--Please Select--"));
            $.each(response, function () {
                Categorydropdown.append($("<option />").val(this.oid).text(this.name));
            });

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    })
}

var submitForm = function (action) {
    
    //set form action based on the method passed in the click handler, update/delete
    var formAction = '/report/' + action;
    //set form action
    $('#reportForm').attr('action', formAction);
    //submit form
    $('#reportForm').submit();
};

$(document).ready(function () {
    CategoryDropDown();
    $(document)
        .on('click', '#btnPDF', function () {
           
            var valid = true;
            if ($('#FromDate').val()=='') {
                $('#FromDateValdiation').show();
                valid = false;
            } else {
                $('#FromDateValdiation').hide();
            }
            if ($('#ToDate').val() == '') {
                $('#ToDateValdiation').show();
                valid = false;
            } else {
                $('#ToDateValdiation').hide();
            }
            if (valid == true) {
                submitForm('print');
            }
        })
        .on('click', '#btnExcel', function () {
            var valid = true;
            if ($('#FromDate').val() == '') {
                $('#FromDateValdiation').show();
                valid = false;
            } else {
                $('#FromDateValdiation').hide();
            }
            if ($('#ToDate').val() == '') {
                $('#ToDateValdiation').show();
                valid = false;
            } else {
                $('#ToDateValdiation').hide();
            }
            if (valid == true) {
                submitForm('print');
            }
            submitForm('export');
        });
})
