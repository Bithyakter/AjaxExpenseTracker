/* For From Date */
$(function () {
    $("#From1").attr("ReadOnly", true)
    $('#From1').datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText) {
            //alert()
            $('#userDOBValidation').hide();
        }
    }).datepicker('option', 'dateFormat', 'dd/mm/yy');
});

/*For To Date */
$(function () {
    $("#Todate").attr("ReadOnly", true)
    $('#Todate').datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText) {
            //alert()
            $('#userDOBValidation').hide();
        }
    }).datepicker('option', 'dateFormat', 'dd/mm/yy');
});